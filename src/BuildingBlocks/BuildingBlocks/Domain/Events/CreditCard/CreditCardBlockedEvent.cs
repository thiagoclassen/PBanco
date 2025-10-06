namespace BuildingBlocks.Domain.Events.CreditCard;

public class CreditCardBlockedEvent : IDomainEvent
{
    public required Guid CreditCardId { get; init; }
    public required string OldStatus { get; init; }
    public Guid EventId { get; init; }
    public string EventName => nameof(CreditCardBlockedEvent);
    public DateTime OccurredOn { get; init; }
}