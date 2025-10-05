using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.RejectProposal;

public record RejectProposalCommand(Guid ProposalId) : ICommand<ErrorOr<RejectProposalResult>>;

public record RejectProposalResult(Models.Proposal Proposal);

public class RejectProposalCommandHandler(IProposalRepository repository)
    : ICommandHandler<RejectProposalCommand, ErrorOr<RejectProposalResult>>
{
    public async Task<ErrorOr<RejectProposalResult>> Handle(RejectProposalCommand command,
        CancellationToken cancellationToken)
    {
        var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);

        if (proposal is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");

        proposal.Reject();

        await repository.UpdateAsync(proposal, cancellationToken);

        return new RejectProposalResult(proposal);
    }
}