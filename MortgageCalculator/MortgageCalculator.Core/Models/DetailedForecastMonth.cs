namespace MortgageCalculator.Core.Models;

public class DetailedForecastMonth
{
    public decimal TMP_CompareMonthInterest { get; set; }
    public List<DetailedForecastDay> Days { get; set; } = new();

    public DateOnly From => Days.First().Date;
    public DateOnly To => Days.Last().Date;
    public decimal PaidIn => Days.Sum(d => d.PaidIn);
    public decimal PaidOut => Days.Sum(d => d.PaidOut);
    public double LoanToValue => Days.Last().LoanToValue;
    public decimal Balance => Days.Last().Balance;
}
