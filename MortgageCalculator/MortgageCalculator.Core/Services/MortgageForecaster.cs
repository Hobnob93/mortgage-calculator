using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Services;

public class MortgageForecaster : IMortgageForecaster
{
    private readonly IMongoRepository<Mortgage> _mortgagesRepo;
    private readonly IMortgagePayments _mortgagePayments;

    public MortgageForecaster(IMongoRepository<Mortgage> mortgagesRepo, IMortgagePayments mortgagePayments)
    {
        _mortgagesRepo = mortgagesRepo;
        _mortgagePayments = mortgagePayments;
    }

    public async Task<DetailedForecast> GetDetailedForecast()
    {
        // TODO: find mortgage based on forecast start-from date, otherwise use start of FIRST mortgage - need to order by mortgage opened date if multiple mortgages exist
        var mortgage = (await _mortgagesRepo.GetAll()).First();

        var currentDate = mortgage.Opened;
        var amountToPayoff = mortgage.AmountBorrowed;
        var monthToMonthCarryOver = 0m;

        var forecast = new DetailedForecast();
        while (amountToPayoff > 0m)
        {
            var paymentsInMonth = (await _mortgagePayments.PaymentsForMonth(mortgage, currentDate, amountToPayoff)).ToArray();
            var forecastForMonth = ForecastNextMonth(ref amountToPayoff, currentDate, mortgage, paymentsInMonth);

            if (monthToMonthCarryOver > 0)
            {
                amountToPayoff += monthToMonthCarryOver;
                monthToMonthCarryOver = 0m;
            }

            if (forecast.Months.Count == 0 && forecastForMonth.To < mortgage.FirstPaymentDate)
            {
                // Undo adding interest in first month as first payment not been done yet
                amountToPayoff -= forecastForMonth.PaidOut;
                monthToMonthCarryOver = forecastForMonth.PaidOut;
            }

            forecast.Months.Add(forecastForMonth);

            currentDate = currentDate.StartOfNextMonth();
        }

        return forecast;
    }

    private DetailedForecastMonth ForecastNextMonth(ref decimal amountToPayOff, DateOnly date, Mortgage mortgage, MortgagePayment[] payments)
    {
        var house = mortgage.House ?? throw new InvalidDataException($"The mortgage '{mortgage.Id}' doesn't contain any house data!");

        var interestPeriod = mortgage.InterestPeriods
            .SingleOrDefault(ip => ip.From <= date && date <= ip.To, mortgage.InterestPeriods.Last());
        var (interestForMonth, interestPerDay) = CalculateDailyInterestForMonth(amountToPayOff, date, interestPeriod, payments);

        var forecastForMonth = new DetailedForecastMonth();
        for (var i = date.Day; i <= date.DaysInMonth(); i++)
        {
            date = new DateOnly(date.Year, date.Month, i);

            var payment = payments
                .Where(p => p.PaidOn == date)
                .Sum(p => p.Amount);

            if (payment > 0m)
                amountToPayOff -= payment;

            forecastForMonth.Days.Add(new DetailedForecastDay
            {
                Date = date,
                PaidIn = payment,
                PaidOut = interestPerDay,
                Balance = amountToPayOff,
                LoanToValue = (double)((amountToPayOff / house.EstimatedValue) * 100m)
            });
        }

        forecastForMonth.TMP_CompareMonthInterest = interestForMonth;
        amountToPayOff += interestForMonth;

        return forecastForMonth;
    }

    private (decimal ForMonth, decimal ForDay) CalculateDailyInterestForMonth(decimal balance, DateOnly interestStart, InterestPeriod interestPeriod, MortgagePayment[] payments)
    {
        var daysInMonth = (interestStart.DaysInMonth() - interestStart.Day) + 1;
        var interestFraction = (interestPeriod.InterestRate / 100m);
        var baseAmount = Math.Round(balance * interestFraction * daysInMonth, 4, MidpointRounding.ToEven);

        var paymentDeductions = 0m;
        foreach (var payment in payments)
        {
            paymentDeductions += Math.Round(payment.Amount * interestFraction * (daysInMonth - payment.PaidOn.Day + 1), 4, MidpointRounding.ToEven);
        }

        var forMonth = Math.Round((baseAmount - paymentDeductions) / (decimal)interestStart.DaysInYear(), 2, MidpointRounding.ToEven);
        var forDay = Math.Round(forMonth / (decimal)daysInMonth, 2, MidpointRounding.ToEven);

        return (forMonth, forDay);
    }
}
