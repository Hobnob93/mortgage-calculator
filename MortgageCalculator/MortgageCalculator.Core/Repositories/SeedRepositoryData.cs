using MongoDB.Driver;
using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Repositories;

public class SeedRepositoryData : MongoRepositoryBase, ISeedRepositoryData
{
	public SeedRepositoryData(IMongoDatabase mongoDatabase)
		: base(mongoDatabase)
	{
	}

    public async Task SeedData()
    {
        
    }
}
