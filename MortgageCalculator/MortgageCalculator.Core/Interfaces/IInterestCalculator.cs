using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Interfaces;

public interface IInterestCalculator
{
    (decimal ForMonth, decimal ForDay) CalculateInterestForMonth(ForecastData forecastData, MortgagePayment[] paymentsInMonth);
}
