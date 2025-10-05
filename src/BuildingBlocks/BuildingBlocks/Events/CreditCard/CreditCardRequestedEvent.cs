using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardRequestedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(CreditCardRequestedEvent);
    public required DateTime OccurredOn { get; init; }
    public required Guid CreditCardId { get; init; }
    public required Guid ClientId { get; init; }
    public required Guid ProposalId { get; init; }
    public required int ExpensesLimit { get; init; }
    public required int CardNumber { get; init; }
    public required string CardStatus { get; init; }
    public required string CardProvider { get; init; }
}