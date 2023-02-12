namespace MortgageCalculator.Core.Documents;

public record MortgagePaymentTo : DocumentBase
{
    public required string Name { get; init; }
    public required DateOnly StartDate { get; init; }
}
