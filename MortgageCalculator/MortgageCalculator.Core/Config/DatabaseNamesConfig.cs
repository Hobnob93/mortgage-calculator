namespace MortgageCalculator.Core.Config;

public class DatabaseNamesConfig
{
    public const string Section = "DatabaseNames";

    public string Mortgage { get; set; } = default!;
}
