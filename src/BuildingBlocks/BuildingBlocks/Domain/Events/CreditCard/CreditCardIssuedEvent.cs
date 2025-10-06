namespace BuildingBlocks.Domain.Events.CreditCard;

public class CreditCardIssuedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(CreditCardIssuedEvent);
    public required Guid CreditCardId { get; init; }
    public required string OldStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}