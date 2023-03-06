using MortgageCalculator.Core.Documents;
using OneOf;
using OneOf.Types;

namespace MortgageCalculator.Core.Interfaces;

public interface IMortgageRepository : IMongoRepository<Mortgage>
{
    Task<Mortgage> GetFirstMortgage();
    Task<Mortgage> GetMostRecentMortgage();
    Task<OneOf<Mortgage, NotFound>> GetMortgageAtDate(DateOnly? date = null);
}
