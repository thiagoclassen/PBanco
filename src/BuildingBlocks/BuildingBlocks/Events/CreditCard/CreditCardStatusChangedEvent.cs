using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardStatusChangedEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public Guid CreditCardId { get; set; }
    public required string NewStatus { get; set; }
    public required string OldStatus { get; set; }
    public DateTime OccurredOn { get; set; }
}