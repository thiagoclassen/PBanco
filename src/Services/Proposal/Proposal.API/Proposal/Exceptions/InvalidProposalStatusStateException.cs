using BuildingBlocks.Domain.Exceptions;

namespace ProposalApi.Proposal.Exceptions;

public class InvalidProposalStatusStateException : DomainException
{
    public InvalidProposalStatusStateException(string newStatus, string oldStatus)
        : base($"Cannot transition to {oldStatus} from {newStatus}.") { }
    
    public InvalidProposalStatusStateException(string status)
        : base($"The card status '{status}' is not a valid state transition.") { }
}