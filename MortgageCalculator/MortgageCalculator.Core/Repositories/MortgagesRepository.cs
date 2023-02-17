using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Repositories;

public class MortgagesRepository : MongoRepositoryBase, IMongoRepository<Mortgage>
{
    private readonly string _collectionName;

    public MortgagesRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
		_collectionName = collectionNames.Value.Mortgages;
	}

	public async Task<IEnumerable<Mortgage>> GetAll()
	{
		return await GetAllFromCollection<Mortgage>(_collectionName);
	}

	public async Task UpdateDocument(Mortgage document)
	{
		await UpdateDocumentInCollection(_collectionName, document);
	}

	public async Task DeleteDocument(Mortgage document)
	{
		await DeleteDocumentInCollection(_collectionName, document);
	}
}
