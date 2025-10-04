using BuildingBlocks.Domain;
using BuildingBlocks.Events.Proposal;
using ProposalApi.Proposal.Model;

namespace ProposalApi.Proposal.Models;

public class Proposal : AggregateRoot
{
    public Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public ProposalStatus ProposalStatus { get; set; } = ProposalStatus.Pending;
    public DateTime Requested { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // public ProposalStatusLookup ProposalStatusLookup { get; set; }
    
    public void UpdateStatus(ProposalStatus newStatus)
    {
        if (ProposalStatus == newStatus) return;
        AddDomainEvent(new ProposalStatusChangedEvent
        {
            Id = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = Id,
            OldStatus = ProposalStatus.ToString(),
            NewStatus = newStatus.ToString()
        });
        ProposalStatus = newStatus;
    }

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
            Id = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ProposalId = proposal.Id,
            ClientId = proposal.ClientId,
            ProposalStatus = proposal.ProposalStatus.ToString()
        });
        return proposal;
    }
}

public enum ProposalStatus
{
    Pending,
    UnderReview,
    Approved,
    Rejected,
    Cancelled
}