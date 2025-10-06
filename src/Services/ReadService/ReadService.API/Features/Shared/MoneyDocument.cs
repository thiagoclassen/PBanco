using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.API.Features.Shared;

public class MoneyDocument
{
    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = "BRL";

    public MoneyDocument(decimal amount, string currency = "BRL")
    {
        Amount = amount;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Currency} {Amount:F2}";
    }
}