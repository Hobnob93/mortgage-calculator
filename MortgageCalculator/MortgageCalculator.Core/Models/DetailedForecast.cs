namespace MortgageCalculator.Core.Models;

public class DetailedForecast
{
    public List<DetailedForecastMonth> Months { get; set; } = new();

    public DateOnly From => Months.First().From;
    public DateOnly To => Months.Last().To;
    public decimal FinalBalance => Months.Last().Balance;
    public double FinalLoanToValue => Months.Last().LoanToValue;
    public decimal TotalPaidIn => Months.Sum(m => m.PaidIn);
    public decimal TotalPaidOut => Months.Sum(m => m.PaidOut);
}
