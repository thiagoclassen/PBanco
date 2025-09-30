using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Models;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.UpdateProposal;

public record UpdateProposalCommand(Guid ProposalId, string NewStatus) : ICommand<ErrorOr<ProposalResponse>>;

public record ProposalResponse(Guid Id, Guid ClientId, string Status, DateTime Requested, DateTime CreatedAt, DateTime UpdatedAt);

public class UpdateProposalCommandHandler(IProposalRepository repository) : ICommandHandler<UpdateProposalCommand, ErrorOr<ProposalResponse>>
{
    public async Task<ErrorOr<ProposalResponse>> Handle(UpdateProposalCommand command, CancellationToken cancellationToken)
    {
        var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);
        if (proposal is null)
            return Error.Validation("Proposal.NotFound", "Proposal not found");

        proposal.ProposalStatus = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), command.NewStatus, true);
        await repository.UpdateAsync(proposal, cancellationToken);
        
        return new ProposalResponse(
            proposal.Id,
            proposal.ClientId,
            proposal.ProposalStatus.ToString(),
            proposal.Requested,
            proposal.CreatedAt,
            proposal.UpdatedAt
        );
    }
}