using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.CancelProposal;

public record CancelProposalCommand(Guid ProposalId) : ICommand<ErrorOr<CancelProposalResult>>;
public record CancelProposalResult(Models.Proposal Proposal);

public class CancelProposalCommandHandler(IProposalRepository repository) : ICommandHandler<CancelProposalCommand, ErrorOr<CancelProposalResult>>
{
    public async Task<ErrorOr<CancelProposalResult>> Handle(CancelProposalCommand command, CancellationToken cancellationToken)
    {
        var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);
        
        if (proposal is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");
        
        proposal.Cancel();
        
        await repository.UpdateAsync(proposal, cancellationToken);
        
        return new CancelProposalResult(proposal);
    }
}