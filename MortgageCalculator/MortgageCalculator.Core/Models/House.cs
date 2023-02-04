namespace MortgageCalculator.Core.Models;

public record House : DocumentBase
{
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public decimal PurchasedValue { get; set; }
    public decimal EstimatedValue { get; set; }
}
