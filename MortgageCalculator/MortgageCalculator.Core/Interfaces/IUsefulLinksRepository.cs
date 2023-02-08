using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Interfaces;

public interface IUsefulLinksRepository
{
    Task<IEnumerable<UsefulLink>> GetLinks();
    Task UpdateLink(UsefulLink link);
}
