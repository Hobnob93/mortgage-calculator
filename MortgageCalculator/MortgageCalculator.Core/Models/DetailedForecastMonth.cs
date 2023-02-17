namespace MortgageCalculator.Core.Models;

public class DetailedForecastMonth
{
    public List<DetailedForecastDay> Days { get; set; } = new();

    public DateOnly Date => Days.First().Date;
    public decimal PaidIn => Days.Sum(d => d.PaidIn);
    public decimal PaidOut => Days.Sum(d => d.PaidOut);
    public double LoanToValue => Days.Last().LoanToValue;
    public decimal Balance => Days.Last().Balance;
}
