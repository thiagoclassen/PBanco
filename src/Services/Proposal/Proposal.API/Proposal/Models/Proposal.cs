using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Events.Proposal;
using BuildingBlocks.Domain.Shared;
using ProposalApi.Proposal.Exceptions;

namespace ProposalApi.Proposal.Models;

public class Proposal : AggregateRoot
{
    private static readonly Dictionary<ProposalStatus, ProposalStatus[]> ValidTransitions =
        new()
        {
            { ProposalStatus.Pending, [ProposalStatus.Approved, ProposalStatus.Rejected] },
            { ProposalStatus.Approved, [] },
            { ProposalStatus.Rejected, [] }
        };

    public Guid Id { get; init; }
    public required Guid CreditCardId { get; init; }
    public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
    public Money ApprovedAmount { get; set; } = new(0.0m);
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public static Proposal Create(Guid creditCardId)
    {
        var proposal = new Proposal
        {
            Id = Guid.NewGuid(),
            CreditCardId = creditCardId,
            Status = ProposalStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        proposal.AddDomainEvent(new ProposalCreatedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = proposal.Id,
            CreditCardId = proposal.CreditCardId,
            ProposalStatus = proposal.Status.ToString()
        });
        proposal.AddDomainEvent(proposal.CreatePropagateEvent());

        return proposal;
    }

    public void Approve(Money amount)
    {
        ValidateTransition(ProposalStatus.Approved);
        Status = ProposalStatus.Approved;
        ApprovedAmount = amount;
        AddDomainEvent(new ProposalApprovedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            CreditCardId = CreditCardId,
            ApprovedAmount = ApprovedAmount
        });
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Reject()
    {
        ValidateTransition(ProposalStatus.Rejected);
        Status = ProposalStatus.Rejected;
        AddDomainEvent(new ProposalRejectedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            CreditCardId = CreditCardId
        });
        AddDomainEvent(CreatePropagateEvent());
    }


    public void UpdateAmount(Money newAmount)
    {
        if (ApprovedAmount == newAmount) return;
        CanUpdateProposalAmount(newAmount.Amount);
        AddDomainEvent(new ProposalAmountUpdateEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            OldAmount = ApprovedAmount,
            NewAmount = newAmount
        });
        ApprovedAmount = newAmount;
        AddDomainEvent(CreatePropagateEvent());
    }

    private ProposalPropagateEvent CreatePropagateEvent()
    {
        return new ProposalPropagateEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ProposalCreditCardId = CreditCardId,
            ProposalStatus = Status.ToString(),
            ProposalApprovedAmount = ApprovedAmount.Amount,
            ProposalApprovedCurrency = ApprovedAmount.Currency,
            ProposalCreatedAt = CreatedAt,
            ProposalUpdatedAt = UpdatedAt
        };
    }

    private bool CanUpdateProposalAmount(decimal newAmount)
    {
        if (newAmount < 0) throw new InvalidProposalUpdateException(newAmount);
        return Status != ProposalStatus.Pending ? throw new InvalidProposalUpdateException(Status) : true;
    }

    private void ValidateTransition(ProposalStatus newStatus)
    {
        if (!ValidTransitions.TryGetValue(Status, out var allowed))
            throw new InvalidProposalStatusStateException(newStatus.ToString());

        if (!allowed.Contains(newStatus))
            throw new InvalidProposalStatusStateException(newStatus.ToString(), Status.ToString());
    }
}

public enum ProposalStatus
{
    Pending,
    Approved,
    Rejected
}