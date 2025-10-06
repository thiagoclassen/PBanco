namespace BuildingBlocks.Domain.Events.CreditCard;

public class CreditCardRejectedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(CreditCardRejectedEvent);
    public required Guid CreditCardId { get; init; }
    public required string OldStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}