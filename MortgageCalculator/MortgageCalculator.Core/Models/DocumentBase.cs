using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MortgageCalculator.Core.Models;

public record DocumentBase
{
    [BsonId, BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
}
