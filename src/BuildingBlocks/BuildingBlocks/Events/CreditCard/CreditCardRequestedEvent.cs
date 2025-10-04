using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.CreditCard;

public class CreditCardRequestedEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public Guid CreditCardId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ProposalId { get; set; }
    public int ExpensesLimit { get; set; }
    public int CardNumber { get; set; }
    public required string CardStatus { get; set; }
    public required string CardProvider { get; set; }   
}