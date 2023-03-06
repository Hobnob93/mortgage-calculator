using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Services;

public class MortgageForecaster : IMortgageForecaster
{
    private readonly IMortgageRepository _mortgageRepo;
    private readonly IMortgagePayments _mortgagePayments;
    private readonly IInterestCalculator _interestCalculator;

    public MortgageForecaster(IMortgageRepository mortgageRepo, IMortgagePayments mortgagePayments, IInterestCalculator interestCalculator)
    {
        _mortgageRepo = mortgageRepo;
        _mortgagePayments = mortgagePayments;
        _interestCalculator = interestCalculator;
    }

    public async Task<DetailedForecast> GetDetailedForecast()
    {
        // TODO: find mortgage based on forecast start-from date, otherwise use start of FIRST mortgage - need to order by mortgage opened date if multiple mortgages exist
        var mortgage = await _mortgageRepo.GetFirstMortgage();

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
        var house = mortgage.House
            ?? throw new InvalidDataException($"The mortgage '{mortgage.Id}' doesn't contain any house data!");

        var (interestForMonth, interestPerDay) = _interestCalculator.CalculateInterestForMonth(amountToPayOff, date, mortgage, payments);
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

        amountToPayOff += interestForMonth;

        return forecastForMonth;
    }
}
