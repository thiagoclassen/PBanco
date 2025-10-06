using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Events.CreditCard;
using BuildingBlocks.Domain.Shared;
using CreditCard.API.CreditCard.Exceptions;

namespace CreditCard.API.CreditCard.Models;

public class CreditCard : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public Money ExpensesLimit { get; set; } = new(0);
    public int? Number { get; set; }
    public CardStatus Status { get; set; } = CardStatus.PendingApproval;
    public CardProvider CardProvider { get; init; } = CardProvider.Visa; // Default provider
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static CreditCard Create(Guid clientId, CardProvider cardProvider = CardProvider.Visa)
    {
        var creditCard = new CreditCard
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            ExpensesLimit = new(0m), // Default limit to 0
            Number = null, // Card number to be assigned when Approved
            Status = CardStatus.PendingApproval, // New cards start as PendingApproval
            CardProvider = cardProvider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        creditCard.AddDomainEvent(new CreditCardCreatedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = creditCard.Id
        });

        creditCard.AddDomainEvent(creditCard.CreatePropagateEvent());

        return creditCard;
    }

    public void IssueCardSetInactive(Money approvedAmmount)
    {
        ValidateTransition(CardStatus.Inactive);
        AddDomainEvent(new CreditCardIssuedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = Status.ToString()
        });
        Status = CardStatus.Inactive;
        ExpensesLimit = approvedAmmount;
        GenerateCardNumber();
        AddDomainEvent(CreatePropagateEvent());
    }
    
    public void RejectCard()
    {
        ValidateTransition(CardStatus.Rejected);
        AddDomainEvent(new CreditCardRejectedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = Status.ToString()
        });
        Status = CardStatus.Rejected;
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Activate()
    {
        ValidateTransition(CardStatus.Active);
        AddDomainEvent(new CreditCardActiveEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = Status.ToString()
        });
        Status = CardStatus.Active;
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Block()
    {
        ValidateTransition(CardStatus.Blocked);
        AddDomainEvent(new CreditCardBlockedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = Status.ToString()
        });
        Status = CardStatus.Blocked;
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Cancel()
    {
        ValidateTransition(CardStatus.Canceled);
        AddDomainEvent(new CreditCardCanceledEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            OldStatus = Status.ToString()
        });
        Status = CardStatus.Canceled;
        AddDomainEvent(CreatePropagateEvent());
    }


    private void GenerateCardNumber()
    {
        if (Number != null) return; // Card number already assigned

        // Simulate card number generation
        var random = new Random();
        Number = random.Next(100000000, 999999999);
    }

    private CreditCardPropagateEvent CreatePropagateEvent()
    {
        return new CreditCardPropagateEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            CreditCardId = Id,
            CreditCardClientId = ClientId,
            CreditCardExpensesLimitAmount = ExpensesLimit.Amount,
            CreditCardExpensesLimitCurrency = ExpensesLimit.Currency,
            CreditCardNumber = Number,
            CardStatus = Status.ToString(),
            CardProvider = CardProvider.ToString(),
            CreditCardCreatedAt = CreatedAt,
            CreditCardUpdatedAt = UpdatedAt
        };
    }

    private static readonly Dictionary<CardStatus, CardStatus[]> ValidTransitions =
        new()
        {
            { CardStatus.PendingApproval, [CardStatus.Inactive, CardStatus.Rejected, CardStatus.Canceled] },
            { CardStatus.Rejected, [] },
            { CardStatus.Inactive, [CardStatus.Active, CardStatus.Canceled] },
            { CardStatus.Active, [CardStatus.Blocked, CardStatus.Canceled] },
            { CardStatus.Blocked, [CardStatus.Active, CardStatus.Canceled] },
            { CardStatus.Canceled, [] }
        };

    private void ValidateTransition(CardStatus newStatus)
    {
        if (!ValidTransitions.TryGetValue(Status, out var allowed))
            throw new InvalidCardStatusStateException(Status.ToString());

        if (!allowed.Contains(newStatus))
            throw new InvalidCardStatusStateException(newStatus.ToString(), Status.ToString());
    }
}

public enum CardStatus
{
    PendingApproval,
    Rejected,
    Inactive,
    Active,
    Blocked,
    Canceled
}

public enum CardProvider
{
    Visa,
    MasterCard,
    AmericanExpress,
    Elo
}