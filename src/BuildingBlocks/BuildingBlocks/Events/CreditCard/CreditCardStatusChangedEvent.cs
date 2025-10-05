using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardStatusChangedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(CreditCardStatusChangedEvent);
    public required Guid CreditCardId { get; init; }
    public required string NewStatus { get; init; }
    public required string OldStatus { get; init; }
    public required DateTime OccurredOn { get; init; }
}