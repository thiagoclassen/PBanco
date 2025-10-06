using BuildingBlocks.Domain.Exceptions;
using ProposalApi.Proposal.Models;

namespace ProposalApi.Proposal.Exceptions;

public class InvalidProposalUpdateException : DomainException
{
    public InvalidProposalUpdateException(ProposalStatus newStatus)
        : base($"Cannot update a proposal with Status '{newStatus.ToString()}'.") { }

    public InvalidProposalUpdateException(decimal newAmount)
        : base($"The value $ '{newAmount}' is invalid.") { }
}