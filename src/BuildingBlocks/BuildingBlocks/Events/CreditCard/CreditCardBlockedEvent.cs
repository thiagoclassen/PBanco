using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardBlockedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(CreditCardBlockedEvent);
    public required Guid CreditCardId { get; init; }
    public required string OldStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}