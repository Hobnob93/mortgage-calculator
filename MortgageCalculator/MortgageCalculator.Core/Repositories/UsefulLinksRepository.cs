using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Repositories;

public class UsefulLinksRepository : MongoRepositoryBase, IUsefulLinksRepository
{
    private readonly CollectionNamesConfig _collectionNames;

    public UsefulLinksRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
		_collectionNames = collectionNames.Value;
	}

	public async Task<IEnumerable<UsefulLink>> GetUsefulLinks()
	{
		return await GetAllFromCollection<UsefulLink>(_collectionNames.UsefulLinks);
	}
}
