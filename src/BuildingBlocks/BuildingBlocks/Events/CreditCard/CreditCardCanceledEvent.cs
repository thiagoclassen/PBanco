using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardCanceledEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(CreditCardCanceledEvent);
    public required Guid CreditCardId { get; init; }
    public DateTime OccurredOn { get; init; }
}