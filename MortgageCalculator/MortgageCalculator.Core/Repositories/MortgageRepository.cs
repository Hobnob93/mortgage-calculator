using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Interfaces;
using OneOf;
using OneOf.Types;

namespace MortgageCalculator.Core.Repositories;

public class MortgageRepository : MongoRepositoryBase, IMortgageRepository
{
    private readonly string _collectionName;

    public MortgageRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
        : base(mongoDatabase)
    {
        _collectionName = collectionNames.Value.Mortgages;
    }

    public async Task DeleteDocument(Mortgage document)
    {
        await DeleteDocumentInCollection(_collectionName, document);
    }

    public async Task<IEnumerable<Mortgage>> GetAll()
    {
        return await GetAllFromCollection<Mortgage>(_collectionName);
    }

    public async Task<Mortgage> GetFirstMortgage()
    {
        var allMortgages = await GetAllFromCollection<Mortgage>(_collectionName);

        return allMortgages.OrderBy(m => m.Opened)
            .First();
    }

    public async Task<OneOf<Mortgage, NotFound>> GetMortgageAtDate(DateOnly? date = null)
    {
        var nonNullDate = date ?? DateOnly.FromDateTime(DateTime.Now);

        var results = await GetFilteredFromCollection<Mortgage>(_collectionName, (m) => MortgageAtDatePredicate(m, nonNullDate));
        if (results.Any())
            return results.Single();

        return new NotFound();
    }

    public async Task<Mortgage> GetMostRecentMortgage()
    {
        var allMortgages = await GetAllFromCollection<Mortgage>(_collectionName);

        return allMortgages.OrderBy(m => m.Opened)
            .Last();
    }

    public async Task UpdateDocument(Mortgage document)
    {
        await UpdateDocumentInCollection(_collectionName, document);
    }

    private static bool MortgageAtDatePredicate(Mortgage mortgage, DateOnly date)
    {
        if (date < mortgage.Opened)
            return false;

        if (mortgage.Closed is not null && date > mortgage.Closed)
            return false;

        return true;
    }
}
