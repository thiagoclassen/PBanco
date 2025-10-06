using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Shared;
using BuildingBlocks.Events.Proposal;

namespace ProposalApi.Proposal.Models;

public class Proposal : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public ProposalStatus ProposalStatus { get; set; } = ProposalStatus.Pending;
    public Money ApprovedAmount { get; set; } = new(0.0m);
    public DateTime Requested { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public static Proposal Create(Guid clientId)
    {
        var proposal = new Proposal
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            ProposalStatus = ProposalStatus.Pending,
            Requested = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        proposal.AddDomainEvent(new ProposalCreatedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = proposal.Id,
            ClientId = proposal.ClientId,
            ProposalStatus = proposal.ProposalStatus.ToString()
        });
        proposal.AddDomainEvent(proposal.CreatePropagateEvent());

        return proposal;
    }

    public void Approve(Money amount)
    {
        ProposalStatus = ProposalStatus.Approved;
        ApprovedAmount = amount;
        AddDomainEvent(new ProposalApprovedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ClientId = ClientId,
            ApprovedAmount = ApprovedAmount
        });
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Cancel()
    {
        ProposalStatus = ProposalStatus.Canceled;
        AddDomainEvent(new ProposalCanceledEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ClientId = ClientId
        });
        AddDomainEvent(CreatePropagateEvent());
    }

    // For when the user is deleted we only propagate the proposal change since the credit card service will also consume the ClientDeletedEvent
    public void CancelWithPropagateEvent()
    {
        ProposalStatus = ProposalStatus.Canceled;
        AddDomainEvent(CreatePropagateEvent());
    }

    public void Reject()
    {
        ProposalStatus = ProposalStatus.Rejected;
        AddDomainEvent(new ProposalRejectedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ClientId = ClientId
        });
        AddDomainEvent(CreatePropagateEvent());
    }

    public void UpdateStatus(ProposalStatus newStatus)
    {
        if (ProposalStatus == newStatus) return;
        AddDomainEvent(new ProposalStatusChangedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            OldStatus = ProposalStatus.ToString(),
            NewStatus = newStatus.ToString()
        });
        ProposalStatus = newStatus;
        AddDomainEvent(CreatePropagateEvent());
    }

    public void UpdateAmount(Money newAmount)
    {
        if (ApprovedAmount == newAmount) return;
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
            ProposalClientId = ClientId,
            ProposalStatus = ProposalStatus.ToString(),
            ProposalApprovedAmount = ApprovedAmount.Amount,
            ProposalApprovedCurrency = ApprovedAmount.Currency,
            ProposalRequested = Requested,
            ProposalCreatedAt = CreatedAt,
            ProposalUpdatedAt = UpdatedAt
        };
    }
}

public enum ProposalStatus
{
    Pending,
    Approved,
    Rejected,
    Canceled
}