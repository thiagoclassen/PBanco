using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Domain.Events.CreditCard;

public class CreditCardCreatedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(CreditCardCreatedEvent);
    public required Guid CreditCardId { get; init; }
    public required DateTime OccurredOn { get; init; }
    
}