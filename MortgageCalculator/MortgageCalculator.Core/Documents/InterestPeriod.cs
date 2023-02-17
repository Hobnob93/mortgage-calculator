namespace MortgageCalculator.Core.Documents;

public record InterestPeriod : DocumentBase
{
    public double InterestRate { get; init; }
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public decimal MonthlyPayment { get; init; }

    public double DailyInterestRate => InterestRate / 365.25;
}
