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

    public async Task<DetailedForecast> GetDetailedForecast(DateOnly? forecastUpTo = null)
    {
        // TODO: find mortgage based on forecast start-from date, otherwise use start of FIRST mortgage - need to order by mortgage opened date if multiple mortgages exist
        var mortgage = await _mortgageRepo.GetFirstMortgage();
        var forecastData = new ForecastData
        {
            Mortgage = mortgage,
            AmountToPayOff = mortgage.AmountBorrowed,
            CurrentForecastDate = mortgage.Opened,
            ForecastTo = forecastUpTo
        };

        var forecast = new DetailedForecast();
        var monthToMonthCarryOver = 0m;
        while (forecastData.AmountToPayOff > 0m)
        {
            if (forecastUpTo is not null && forecastData.CurrentForecastDate > forecastUpTo)
                break;

            var paymentsInMonth = (await _mortgagePayments.PaymentsForMonth(forecastData)).ToArray();
            var forecastForMonth = ForecastNextMonth(ref forecastData, paymentsInMonth);

            if (monthToMonthCarryOver > 0)
            {
                forecastData = forecastData with
                {
                    AmountToPayOff = forecastData.AmountToPayOff + monthToMonthCarryOver,
                };
                monthToMonthCarryOver = 0m;
            }

            if (forecast.Months.Count == 0 && forecastForMonth.To < mortgage.FirstPaymentDate)
            {
                // Undo adding interest in first month as first payment not been done yet
                forecastData = forecastData with
                {
                    AmountToPayOff = forecastData.AmountToPayOff - forecastForMonth.PaidOut
                };
                monthToMonthCarryOver = forecastForMonth.PaidOut;
            }

            forecast.Months.Add(forecastForMonth);

            forecastData = forecastData with
            {
                CurrentForecastDate = forecastData.CurrentForecastDate.StartOfNextMonth(),
            };
        }

        return forecast;
    }

    public async Task<SimpleForecast> GetSimpleForecast(DateOnly date)
    {
        var detailedForecast = await GetDetailedForecast(date);

        return new SimpleForecast
        {
            Date = detailedForecast.To,
            Balance = detailedForecast.FinalBalance,
            LoanToValue = detailedForecast.FinalLoanToValue,
            PaidInToDate = detailedForecast.TotalPaidIn,
            PaidOutToDate = detailedForecast.TotalPaidOut
        };
    }

    private DetailedForecastMonth ForecastNextMonth(ref ForecastData forecastData, MortgagePayment[] payments)
    {
        var house = forecastData.Mortgage.House
            ?? throw new InvalidDataException($"The mortgage '{forecastData.Mortgage.Id}' doesn't contain any house data!");

        var (interestForMonth, interestPerDay) = _interestCalculator.CalculateInterestForMonth(forecastData, payments);
        var forecastForMonth = new DetailedForecastMonth();

        var date = forecastData.CurrentForecastDate;
        for (var i = date.Day; i <= date.DaysInMonth(); i++)
        {
            date = new DateOnly(date.Year, date.Month, i);

            var payment = payments
                .Where(p => p.PaidOn == date)
                .Sum(p => p.Amount);

            if (payment > 0m)
            {
                forecastData = forecastData with
                {
                    AmountToPayOff = forecastData.AmountToPayOff - payment,
                };
            }

            forecastForMonth.Days.Add(new DetailedForecastDay
            {
                Date = date,
                PaidIn = payment,
                PaidOut = interestPerDay,
                Balance = forecastData.AmountToPayOff,
                LoanToValue = (double)((forecastData.AmountToPayOff / house.EstimatedValue) * 100m)
            });

            if (forecastData.ForecastTo is not null && date == forecastData.ForecastTo)
                break;
        }

        forecastData = forecastData with
        {
            AmountToPayOff = forecastData.AmountToPayOff + interestForMonth
        };

        return forecastForMonth;
    }
}
