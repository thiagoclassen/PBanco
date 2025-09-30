using ProposalApi.Proposal.Model;

namespace ProposalApi.Proposal.Models;

public class Proposal
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public ProposalStatus ProposalStatus { get; set; } = ProposalStatus.Pending;
    public DateTime Requested { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // public ProposalStatusLookup ProposalStatusLookup { get; set; }
}

public enum ProposalStatus
{
    Pending,
    UnderReview,
    Approved,
    Rejected,
    Cancelled
}