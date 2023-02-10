using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Enums;

namespace MortgageCalculator.Core.Models;

public record EditUsefulLinkResult
{
    public required EditResult EditResult { get; set; }
    public required UsefulLink UsefulLink { get; set; }
}
