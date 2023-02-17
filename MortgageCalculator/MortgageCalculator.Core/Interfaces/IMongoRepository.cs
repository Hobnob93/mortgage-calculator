using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Interfaces;

public interface IMongoRepository<T> where T : DocumentBase
{
    Task<IEnumerable<T>> GetAll();
    Task UpdateDocument(T document);
    Task DeleteDocument(T document);
}
