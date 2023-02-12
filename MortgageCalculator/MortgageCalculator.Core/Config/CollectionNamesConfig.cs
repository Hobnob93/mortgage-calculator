namespace MortgageCalculator.Core.Config;

public class CollectionNamesConfig
{
    public const string Section = "CollectionNames";

    public string Houses { get; set; } = default!;
    public string Mortgages { get; set; } = default!;
    public string MortgagePayments { get; set; } = default!;
    public string Owners { get; set; } = default!;
    public string UsefulLinks { get; set; } = default!;
    public string InterestPeriods { get; set; } = default!;
}
