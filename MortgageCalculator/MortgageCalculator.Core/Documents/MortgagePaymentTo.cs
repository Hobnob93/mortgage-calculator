namespace MortgageCalculator.Core.Documents;

public record MortgagePaymentTo : DocumentBase
{
    public required string Provider { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly FixedTermEndDate { get; init; }
}
