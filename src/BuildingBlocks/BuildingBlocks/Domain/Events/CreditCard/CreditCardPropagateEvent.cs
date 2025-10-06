namespace BuildingBlocks.Domain.Events.CreditCard;

public class CreditCardPropagateEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(CreditCardPropagateEvent);
    public Guid CreditCardId { get; init; }
    public required Guid CreditCardClientId { get; init; }
    public required decimal CreditCardExpensesLimitAmount { get; init; }
    public required string CreditCardExpensesLimitCurrency { get; init; }
    public required int? CreditCardNumber { get; init; }
    public required string CardStatus { get; init; } 
    public required string CardProvider { get; init; }
    public required DateTime CreditCardCreatedAt { get; init; }
    public required DateTime CreditCardUpdatedAt { get; init; }
    public required DateTime OccurredOn { get; init; }
}