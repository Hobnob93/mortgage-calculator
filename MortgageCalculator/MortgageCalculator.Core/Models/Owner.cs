namespace MortgageCalculator.Core.Models;

public record Owner : DocumentBase
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
