using BuildingBlocks.Domain;
using BuildingBlocks.Events.CreditCard;

namespace CreditCard.API.CreditCard.Models;

public class CreditCard : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public required Guid ProposalId { get; init; }
    public int ExpensesLimit { get; set; } = 0;
    public int Number { get; set; } = 0;
    public CardStatus CardStatus { get; set; }
    public CardProvider CardProvider { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public void UpdateStatus(CardStatus newStatus)
    {
        if (CardStatus == newStatus) return;
        AddDomainEvent(new CreditCardStatusChangedEvent()
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = CardStatus.ToString(),
            NewStatus = newStatus.ToString()
        });
        CardStatus = newStatus;
    }
    
    public static CreditCard Create(Guid clientId, Guid proposalId, int expensesLimit, CardProvider cardProvider)
    {
        var random = new Random();
        var cardNumber = random.Next(100000000, 999999999); // Simulate card number generation
        
        var creditCard = new CreditCard
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            ProposalId = proposalId,
            ExpensesLimit = expensesLimit,
            Number = cardNumber,
            CardStatus = CardStatus.Active,
            CardProvider = cardProvider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        creditCard.AddDomainEvent(new CreditCardRequestedEvent()
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = creditCard.Id,
            ClientId = creditCard.ClientId,
            ProposalId = creditCard.ProposalId,
            ExpensesLimit = creditCard.ExpensesLimit,
            CardNumber = creditCard.Number,
            CardStatus = creditCard.CardStatus.ToString(),
            CardProvider = creditCard.CardProvider.ToString()
        });
        
        return creditCard;
    }
    
}

public enum CardStatus
{
    Active,
    Inactive,
    Blocked,
    Cancelled
}

public enum CardProvider
{
    Visa,
    MasterCard,
    AmericanExpress,
    Elo
}