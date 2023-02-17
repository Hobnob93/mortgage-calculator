namespace MortgageCalculator.Core.Documents;

public record Mortgage : DocumentBase
{
    public required string Name { get; set; }
    public required string Provider { get; set; }
    public DateOnly Opened { get; set; }
    public DateOnly? Closed { get; set; }
    public int FullTermLength { get; set; }
    public decimal AmountBorrowed { get; set; }
    public decimal FirstPaymentAmount { get; set; }
    public DateOnly FirstPaymentDate { get; set; }
    public House? House { get; set; }
    public List<InterestPeriod> InterestPeriods { get; set; } = new();
}
