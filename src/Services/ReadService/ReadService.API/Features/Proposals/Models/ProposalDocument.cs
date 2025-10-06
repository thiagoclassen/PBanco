using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReadService.API.Features.Shared;

namespace ReadService.API.Features.Proposals.Models;

public class ProposalDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }

    [BsonElement("creditCardId")]
    [BsonRepresentation(BsonType.String)]
    public Guid CreditCardId { get; init; }

    [BsonElement("proposalStatus")] public string ProposalStatus { get; set; } = "Pending";

    [BsonElement("approvedAmount")] public MoneyDocument ApprovedAmount { get; set; } = new(0);

    [BsonElement("requested")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Requested { get; init; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; init; }

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
}