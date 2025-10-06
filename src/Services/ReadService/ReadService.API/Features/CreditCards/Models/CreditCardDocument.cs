using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReadService.API.Features.Shared;

namespace ReadService.API.Features.CreditCards.Models;

public class CreditCardDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }

    [BsonElement("clientId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ClientId { get; init; }

    [BsonElement("expensesLimit")]
    public MoneyDocument ExpensesLimit { get; set; } = new MoneyDocument(0);

    [BsonElement("number")]
    public int? Number { get; set; }

    [BsonElement("cardStatus")]
    public string CardStatus { get; set; } = "Inactive";

    [BsonElement("cardProvider")]
    public string CardProvider { get; init; } = "Visa";

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
}