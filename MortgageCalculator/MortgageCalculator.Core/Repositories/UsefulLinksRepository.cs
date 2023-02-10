using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Repositories;

public class UsefulLinksRepository : MongoRepositoryBase, IUsefulLinksRepository
{
    private readonly CollectionNamesConfig _collectionNames;

    public UsefulLinksRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
		_collectionNames = collectionNames.Value;
	}

	public async Task<IEnumerable<UsefulLink>> GetLinks()
	{
		return await GetAllFromCollection<UsefulLink>(_collectionNames.UsefulLinks);
	}

	public async Task UpdateLink(UsefulLink link)
	{
		await UpdateDocumentInCollection(_collectionNames.UsefulLinks, link);
	}

	public async Task DeleteLink(UsefulLink link)
	{
		await DeleteDocumentInCollection(_collectionNames.UsefulLinks, link);
	}
}
