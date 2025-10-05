using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.ApproveProposal;

public record ApproveProposalCommand(Guid ProposalId, int ApprovedAmount) : ICommand<ErrorOr<ApproveProposalResult>>;
public record ApproveProposalResult(Models.Proposal Proposal);

public class ApproveProposalCommandHandler(
    IProposalRepository repository) : ICommandHandler<ApproveProposalCommand, ErrorOr<ApproveProposalResult>>
{
    public async Task<ErrorOr<ApproveProposalResult>> Handle(ApproveProposalCommand command, CancellationToken cancellationToken)
    {
        var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);

        if (proposal is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");
        
        proposal.Approve(command.ApprovedAmount);
        
        await repository.UpdateAsync(proposal, cancellationToken);
        
        return new ApproveProposalResult(proposal);
    }
}