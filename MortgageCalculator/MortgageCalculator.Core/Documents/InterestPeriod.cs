namespace MortgageCalculator.Core.Documents;

public record InterestPeriod : DocumentBase
{
    public decimal InterestRate { get; init; }
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public decimal MonthlyPayment { get; init; }
}
