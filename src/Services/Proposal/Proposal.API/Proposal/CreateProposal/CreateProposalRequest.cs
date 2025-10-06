using BuildingBlocks.Domain.Shared;

namespace ProposalApi.Proposal.CreateProposal;

public class CreateProposalRequest
{
    public Guid ClientId { get; set; }
}