namespace MortgageCalculator.Core.Config;

public class ApiEndpointConfig
{
    public const string Section = "ApiEndpoints";

    public string BaseUrl { get; set; } = default!;
    public string UsefulLinks { get; set; } = default!;
    public string DetailedForecast { get; set; } = default!;
}
