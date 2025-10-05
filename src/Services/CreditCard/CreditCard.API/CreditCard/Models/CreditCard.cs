using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Shared;
using BuildingBlocks.Events.CreditCard;

namespace CreditCard.API.CreditCard.Models;

public class CreditCard : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public required Guid ProposalId { get; init; }
    public Money ExpensesLimit { get; set; } = new(0);
    public int Number { get; set; }
    public CardStatus CardStatus { get; set; }
    public CardProvider CardProvider { get; init; } = CardProvider.Visa; // Default provider
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void UpdateStatus(CardStatus newStatus)
    {
        if (CardStatus == newStatus) return;
        AddDomainEvent(new CreditCardStatusChangedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = CardStatus.ToString(),
            NewStatus = newStatus.ToString()
        });
        CardStatus = newStatus;
    }

    public static CreditCard Create(Guid clientId, Guid proposalId, Money expensesLimit, CardProvider cardProvider)
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
            CardStatus = CardStatus.Inactive, // New cards start as Inactive
            CardProvider = cardProvider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        creditCard.AddDomainEvent(new CreditCardCreatedEvent
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

    public void Cancel()
    {
        if (CardStatus == CardStatus.Cancelled) return;
        AddDomainEvent(new CreditCardCanceledEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = CardStatus.ToString()
        });
        CardStatus = CardStatus.Cancelled;
    }

    public void Block()
    {
        if (CardStatus == CardStatus.Blocked) return;
        AddDomainEvent(new CreditCardBlockedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = CardStatus.ToString()
        });
        CardStatus = CardStatus.Blocked;
    }

    public void Activate()
    {
        if (CardStatus == CardStatus.Active) return;
        AddDomainEvent(new CreditCardActiveEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = CardStatus.ToString()
        });
        CardStatus = CardStatus.Active;
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