using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Repositories;

public class UsefulLinksRepository : MongoRepositoryBase, IMongoRepository<UsefulLink>
{
    private readonly string _collectionName;

    public UsefulLinksRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
		_collectionName = collectionNames.Value.UsefulLinks;
	}

	public async Task<IEnumerable<UsefulLink>> GetAll()
	{
		return await GetAllFromCollection<UsefulLink>(_collectionName);
	}

	public async Task UpdateDocument(UsefulLink document)
	{
		await UpdateDocumentInCollection(_collectionName, document);
	}

	public async Task DeleteDocument(UsefulLink document)
	{
		await DeleteDocumentInCollection(_collectionName, document);
	}
}
