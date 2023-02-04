namespace MortgageCalculator.Core.Models;

public record Mortgage : DocumentBase
{
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly FixedTermEndDate { get; set; }
    public int FullTermLength { get; set; }
    public int FixedTermLength { get; set; }
    public bool HasRemortgaged { get; set; }
    public double InterestRate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public House? House { get; set; }
}
