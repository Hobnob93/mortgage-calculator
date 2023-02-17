namespace MortgageCalculator.Core.Documents;

public record BankHoliday : DocumentBase
{
    public DateOnly Date { get; set; }
    public required string Name { get; set; }
}
