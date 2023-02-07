namespace MortgageCalculator.Core.Models;

public record UsefulLink : DocumentBase
{
    public required string Name { get; init; }
    public required string Icon { get; init; }
    public required string Href { get; init; }
}
