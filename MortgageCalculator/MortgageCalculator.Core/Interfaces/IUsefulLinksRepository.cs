using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Interfaces;

public interface IUsefulLinksRepository
{
    Task<IEnumerable<UsefulLink>> GetUsefulLinks();
}
