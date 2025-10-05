using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.UpdateProposal;

public record UpdateProposalCommand(Guid ProposalId, decimal Amount) : ICommand<ErrorOr<ProposalResponse>>;

public record ProposalResponse(Guid Id, Guid ClientId, string Status, string ApprovedAmount, DateTime Requested, DateTime CreatedAt, DateTime UpdatedAt);

public class UpdateProposalCommandHandler(IProposalRepository repository) : ICommandHandler<UpdateProposalCommand, ErrorOr<ProposalResponse>>
{
    public async Task<ErrorOr<ProposalResponse>> Handle(UpdateProposalCommand command, CancellationToken cancellationToken)
    {
        var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);
        if (proposal is null)
            return Error.Validation("Proposal.NotFound", "Proposal not found");
        
        proposal.UpdateAmount(new Money(command.Amount));
        
        await repository.UpdateAsync(proposal, cancellationToken);
        
        return new ProposalResponse( 
            proposal.Id,
            proposal.ClientId,
            proposal.ProposalStatus.ToString(),
            proposal.ApprovedAmount.ToString(),
            proposal.Requested,
            proposal.CreatedAt,
            proposal.UpdatedAt
        );
    }
}