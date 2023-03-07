using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Services;

public class InterestCalculator : IInterestCalculator
{
    public (decimal ForMonth, decimal ForDay) CalculateInterestForMonth(ForecastData forecastData, MortgagePayment[] paymentsInMonth)
    {
        var interestStart = forecastData.CurrentForecastDate;
        var interestPeriod = forecastData.Mortgage.InterestPeriods
            .SingleOrDefault(ip => ip.From <= interestStart && interestStart <= ip.To, forecastData.Mortgage.InterestPeriods.Last());

        var daysInMonth = (interestStart.DaysInMonth() - interestStart.Day) + 1;
        var interestFraction = (interestPeriod.InterestRate / 100m);
        var baseAmount = Math.Round(forecastData.AmountToPayOff * interestFraction * daysInMonth, 4, MidpointRounding.ToEven);

        var paymentDeductions = 0m;
        foreach (var payment in paymentsInMonth)
        {
            paymentDeductions += Math.Round(payment.Amount * interestFraction * (daysInMonth - payment.PaidOn.Day + 1), 4, MidpointRounding.ToEven);
        }

        var forMonth = Math.Round((baseAmount - paymentDeductions) / (decimal)interestStart.DaysInYear(), 2, MidpointRounding.ToEven);
        var forDay = Math.Round(forMonth / (decimal)daysInMonth, 2, MidpointRounding.ToEven);

        var lastDayOfMonth = interestStart.LastDayOfMonth();
        if (forecastData.ForecastTo is not null && forecastData.ForecastTo <= lastDayOfMonth)
        {
            var daysDifference = lastDayOfMonth.DayNumber - forecastData.ForecastTo.Value.DayNumber;
            forMonth -= daysDifference * forDay;
        }

        return (forMonth, forDay);
    }
}
