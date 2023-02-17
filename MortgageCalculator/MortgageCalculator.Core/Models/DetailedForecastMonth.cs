namespace MortgageCalculator.Core.Models;

public class DetailedForecastMonth
{
    public decimal Balance { get; set; }
    public double LoanToValue { get; set; }
    public decimal PaidIn { get; set; }
    public decimal PaidOut { get; set; }
    public List<DetailedForecastDay> Days { get; set; } = new();

    public DateOnly Date => Days.First().Date;
}
