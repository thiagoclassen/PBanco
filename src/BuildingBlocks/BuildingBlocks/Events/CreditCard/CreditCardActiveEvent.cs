using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardActiveEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(CreditCardActiveEvent);
    public required Guid CreditCardId { get; init; }
    public required string OldStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}