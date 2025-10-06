using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.API.Features.ClientCreditCardView.Models;

public class ClientCreditCardViewDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid ClientId { get; init; }

    public string Name { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public List<CardSummary> CreditCards { get; set; } = new();
}

public class CardSummary
{
    public int Number { get; set; }
    public string Provider { get; set; } = null!;
    public string Status { get; set; } = null!;
}