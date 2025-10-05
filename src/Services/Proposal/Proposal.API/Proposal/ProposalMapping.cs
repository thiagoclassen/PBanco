using ProposalApi.Proposal.Models;
using ProposalApi.Proposal.UpdateProposal;

namespace ProposalApi.Proposal;

public static class ProposalMapping
{
    public static ProposalResponse MapToProposalResponse(this Models.Proposal proposal) =>
        new ProposalResponse(proposal.Id, 
            proposal.ClientId, 
            proposal.ProposalStatus.ToString(),
            proposal.ApprovedAmount.ToString(),
            proposal.Requested, 
            proposal.CreatedAt, 
            proposal.UpdatedAt);
}