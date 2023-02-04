using MongoDB.Driver;
using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Repositories;

public class MortgageRepository : MongoRepositoryBase, IMortgageRepository
{
	public MortgageRepository(IMongoDatabase mongoDatabase)
		: base(mongoDatabase)
	{
	}


}
