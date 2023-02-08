using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MortgageCalculator.Core.Documents;

public record DocumentBase
{
    [BsonId]
    public string Id { get; init; } = Guid.NewGuid().ToString();
}
