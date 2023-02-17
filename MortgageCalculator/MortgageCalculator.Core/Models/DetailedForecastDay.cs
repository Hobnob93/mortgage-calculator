namespace MortgageCalculator.Core.Models;

public class DetailedForecastDay
{
    public DateOnly Date { get; set; }
    public decimal Balance { get; set; }
    public double LoanToValue { get; set; }
    public decimal PaidIn { get; set; }
    public decimal PaidOut { get; set; }
}
