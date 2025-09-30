using BuildingBlocks.Domain;
using BuildingBlocks.Events.Proposal;
using ProposalApi.Proposal.Model;

namespace ProposalApi.Proposal.Models;

public class Proposal : AggregateRoot
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public ProposalStatus ProposalStatus { get; set; } = ProposalStatus.Pending;
    public DateTime Requested { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // public ProposalStatusLookup ProposalStatusLookup { get; set; }
    
    public void UpdateStatus(ProposalStatus newStatus)
    {
        if (ProposalStatus == newStatus) return;
        AddDomainEvent(new ProposalStatusChanged
        {
            Id = Guid.NewGuid(),
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
    UnderReview,
    Approved,
    Rejected,
    Cancelled
}