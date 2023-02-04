namespace MortgageCalculator.Core.Models;

public record MortgagePaymentTo : DocumentBase
{
    public required string Provider { get; init; }
    public DateOnly StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
