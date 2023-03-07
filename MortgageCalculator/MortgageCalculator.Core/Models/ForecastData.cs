using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Models;

public record ForecastData
{
    public DateOnly CurrentForecastDate { get; set; }
    public DateOnly? ForecastTo { get; set; }
    public decimal AmountToPayOff { get; set; }
    public required Mortgage Mortgage { get; init; }
}
