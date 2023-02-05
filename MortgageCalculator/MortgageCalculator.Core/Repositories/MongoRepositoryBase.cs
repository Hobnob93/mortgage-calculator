using MongoDB.Driver;
using MortgageCalculator.Core.Models;
using System.Linq.Expressions;

namespace MortgageCalculator.Core.Repositories;

public class MongoRepositoryBase
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoRepositoryBase(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    protected async Task<IEnumerable<T>> GetAllFromCollection<T>(string collectionName) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        var results = await collection.FindAsync(_ => true);
        return results.ToEnumerable();
    }

    protected async Task<IEnumerable<T>> GetFilteredFromCollection<T>(string collectionName, Expression<Func<T, bool>> predicate) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        var results = await collection.FindAsync(predicate);
        return results.ToEnumerable();
    }

    protected async Task<T> GetSingleFromCollection<T>(string collectionName, Expression<Func<T, bool>> predicate) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        var results = await collection.FindAsync(predicate);
        return results.Single();
    }

    protected async Task CreateDocumentInCollection<T>(string collectionName, T newDocument) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        await collection.InsertOneAsync(newDocument);
    }

    protected async Task CreateDocumentsInCollection<T>(string collectionName, params T[] newDocuments) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        await collection.InsertManyAsync(newDocuments);
    }

    protected async Task UpdateDocumentInCollection<T>(string collectionName, T updatedDocument) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq(nameof(DocumentBase.Id), updatedDocument.Id);
        await collection.ReplaceOneAsync(filter, updatedDocument, new ReplaceOptions { IsUpsert = true });
    }

    protected async Task DeleteDocumentInCollection<T>(string collectionName, T documentToDelete) where T : DocumentBase
    {
        var collection = _mongoDatabase.GetCollection<T>(collectionName);
        await collection.DeleteOneAsync(d => d.Id == documentToDelete.Id);
    }
}
