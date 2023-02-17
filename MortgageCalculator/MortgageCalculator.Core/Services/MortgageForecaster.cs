using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Services;

public class MortgageForecaster : IMortgageForecaster
{
    private readonly IMongoRepository<Mortgage> _mortgagesRepo;
    private readonly IMortgagePaymentsRepository _mortgagePaymentsRepo;

    public MortgageForecaster(IMongoRepository<Mortgage> mortgagesRepo, IMortgagePaymentsRepository mortgagePaymentsRepo)
    {
        _mortgagesRepo = mortgagesRepo;
        _mortgagePaymentsRepo = mortgagePaymentsRepo;
    }

    public async Task<DetailedForecast> GetDetailedForecast()
    {
        // TODO: find mortgage based on forecast start-from date, otherwise use start of FIRST mortgage - need to order by mortgage opened date if multiple mortgages exist
        var mortgage = (await _mortgagesRepo.GetAll()).First();

        var currentDate = mortgage.Opened;
        var amountToPayoff = mortgage.AmountBorrowed;

        var forecast = new DetailedForecast();

        while (amountToPayoff > 0m)
        {
            var paymentsInMonth = (await _mortgagePaymentsRepo.PaymentsInMonth(currentDate)).ToList();
            forecast.Months.Add(ForecastNextMonth(ref amountToPayoff, currentDate, mortgage, paymentsInMonth));

            currentDate = currentDate.StartOfNextMonth();
        }

        return forecast;
    }

    private DetailedForecastMonth ForecastNextMonth(ref decimal amountToPayOff, DateOnly date, Mortgage mortgage, List<MortgagePayment> payments)
    {
        var house = mortgage.House ?? throw new InvalidDataException($"The mortgage '{mortgage.Id}' doesn't contain any house data!");
        var interestPeriod = mortgage.InterestPeriods
            .SingleOrDefault(ip => ip.From <= date && date <= ip.To, mortgage.InterestPeriods.Last());

        if (mortgage.FirstPaymentDate == date)
            payments.Add(new MortgagePayment { Amount = mortgage.FirstPaymentAmount });

        var forecastForMonth = new DetailedForecastMonth();

        for (var i = 1; i <= date.DaysInMonth(); i++)
        {
            date = new DateOnly(date.Year, date.Month, i);

            var interestAmount = amountToPayOff * (decimal)(interestPeriod.DailyInterestRate / 100.0);
            var payment = payments
                .Where(p => p.PaidOn == date)
                .Sum(p => p.Amount);

            amountToPayOff = (amountToPayOff + interestAmount) - payment;

            forecastForMonth.Days.Add(new DetailedForecastDay
            {
                Date = date,
                PaidIn = payment,
                PaidOut = interestAmount,
                Balance = amountToPayOff,
                LoanToValue = (double)((amountToPayOff / house.EstimatedValue) * 100m)
            });
        }

        return forecastForMonth;
    }
}
