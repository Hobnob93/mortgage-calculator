namespace MortgageCalculator.Core.Config;

public class ConnectionStringsConfig
{
    public const string Section = "ConnectionStrings";

    public string Local { get; set; } = default!;
}
