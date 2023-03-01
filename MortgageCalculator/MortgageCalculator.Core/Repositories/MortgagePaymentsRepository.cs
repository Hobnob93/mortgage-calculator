using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using System.Linq.Expressions;

namespace MortgageCalculator.Core.Repositories;

public class MortgagePaymentsRepository : MongoRepositoryBase, IMortgagePaymentsRepository
{
    private readonly string _collectionName;

    public MortgagePaymentsRepository(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
		_collectionName = collectionNames.Value.MortgagePayments;
	}

	public async Task<IEnumerable<MortgagePayment>> GetAll()
	{
		return await GetAllFromCollection<MortgagePayment>(_collectionName);
	}

	public async Task UpdateDocument(MortgagePayment document)
	{
		await UpdateDocumentInCollection(_collectionName, document);
	}

	public async Task DeleteDocument(MortgagePayment document)
	{
		await DeleteDocumentInCollection(_collectionName, document);
	}

	public async Task<IEnumerable<MortgagePayment>> PaymentsInMonth(DateOnly date)
	{
		var (firstOfMonth, lastOfMonth) = date.FirstAndLastDaysOfMonth();
		return await GetFilteredFromCollection(_collectionName, DateRangePredicate(firstOfMonth, lastOfMonth));
    }

    private static Expression<Func<MortgagePayment, bool>> DateRangePredicate(DateOnly from, DateOnly to)
	{
		return (mp) => mp.PaidOn >= from && mp.PaidOn <= to;
	}
}
