using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Interfaces;

public interface IMortgagePayments
{
    Task<IEnumerable<MortgagePayment>> PaymentsForMonth(ForecastData forecastData);
}
