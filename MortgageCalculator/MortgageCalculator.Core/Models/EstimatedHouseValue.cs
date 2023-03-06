namespace MortgageCalculator.Core.Models;

public class EstimatedHouseValue
{
    public decimal PurchaseValue { get; set; }
    public decimal EstimatedValue { get; set; }
    public decimal Equity { get; set; }
    public double PercentageDifference { get; set; }
}
