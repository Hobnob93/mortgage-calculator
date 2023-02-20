namespace MortgageCalculator.Core.Models;

public class DetailedForecast
{
    public List<DetailedForecastMonth> Months { get; set; } = new();

    public DateOnly From => Months.First().From;
    public DateOnly To => Months.Last().To;
}
