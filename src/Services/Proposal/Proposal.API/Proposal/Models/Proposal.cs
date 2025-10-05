using BuildingBlocks.Domain;
using BuildingBlocks.Events.Proposal;
using ProposalApi.Proposal.Model;

namespace ProposalApi.Proposal.Models;

public class Proposal : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public ProposalStatus ProposalStatus { get; set; } = ProposalStatus.Pending;
    public int ApprovedAmount { get; set; } = 0;
    public DateTime Requested { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
    
    // public ProposalStatusLookup ProposalStatusLookup { get; set; }
    
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
        return proposal;
    }
   
    public void Approve(int amount)
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
    }
    
    public void Cancel()
    {
        ProposalStatus = ProposalStatus.Cancelled;
        ApprovedAmount = 0;
        AddDomainEvent(new ProposalCanceledEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ClientId = ClientId,
            ApprovedAmount = 0
        });
    }
    
    public void Reject()
    {
        ProposalStatus = ProposalStatus.Rejected;
        ApprovedAmount = 0;
        AddDomainEvent(new ProposalRejectedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            ClientId = ClientId,
            ApprovedAmount = 0
        });
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
    }

}

public enum ProposalStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}