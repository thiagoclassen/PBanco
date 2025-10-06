using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Exceptions;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.RejectProposal;

public record RejectProposalCommand(Guid ProposalId) : ICommand<ErrorOr<RejectProposalResult>>;

public record RejectProposalResult(
    Guid Id,
    Guid CreditCardId,
    string Status,
    string ApprovedAmount,
    DateTime UpdatedAt);

public class RejectProposalCommandHandler(IProposalRepository repository)
    : ICommandHandler<RejectProposalCommand, ErrorOr<RejectProposalResult>>
{
    public async Task<ErrorOr<RejectProposalResult>> Handle(RejectProposalCommand command,
        CancellationToken cancellationToken)
    {
        Models.Proposal? proposal;
        try
        {
            proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);

            if (proposal is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");

            proposal.Reject();

            await repository.UpdateAsync(proposal, cancellationToken);
        }
        catch (InvalidProposalStatusStateException e)
        {
            return Error.Forbidden("Proposal.InvalidState", e.Message);
        }


        return new RejectProposalResult(proposal.Id, proposal.CreditCardId, proposal.Status.ToString(),
            proposal.ApprovedAmount.ToString(), proposal.UpdatedAt);
    }
}