namespace MortgageCalculator.Core.Models;

public class SimpleForecast
{
    public DateOnly Date { get; set; }
    public decimal Balance { get; set; }
    public double LoanToValue { get; set; }
    public decimal PaidInToDate { get; set; }
    public decimal PaidOutToDate { get; set; }
}
