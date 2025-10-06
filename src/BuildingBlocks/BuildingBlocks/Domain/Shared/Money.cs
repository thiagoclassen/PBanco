namespace BuildingBlocks.Domain.Shared;

public sealed record Money
{
    public Money(decimal amount, string currency = "BRL")
    {
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public override string ToString()
    {
        return $"{Currency} {Amount:N2}";
    }
}