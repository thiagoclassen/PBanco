using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using ProposalApi.Proposal.Exceptions;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.UpdateProposal;

public record UpdateProposalCommand(Guid ProposalId, decimal Amount) : ICommand<ErrorOr<ProposalResponse?>>;

public record ProposalResponse(
    Guid Id,
    Guid CreditCardId,
    string Status,
    string ApprovedAmount,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class UpdateProposalCommandHandler(IProposalRepository repository)
    : ICommandHandler<UpdateProposalCommand, ErrorOr<ProposalResponse?>>
{
    public async Task<ErrorOr<ProposalResponse?>> Handle(UpdateProposalCommand command,
        CancellationToken cancellationToken)
    {
        ProposalResponse? response = null;
        try
        {
            var proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);
            if (proposal is null)
                return Error.Validation("Proposal.NotFound", "Proposal not found");

            proposal.UpdateAmount(new Money(command.Amount));

            await repository.UpdateAsync(proposal, cancellationToken);
            
            response = new ProposalResponse(
                proposal.Id,
                proposal.CreditCardId,
                proposal.Status.ToString(),
                proposal.ApprovedAmount.ToString(),
                proposal.CreatedAt,
                proposal.UpdatedAt
            );
        }
        catch (InvalidProposalUpdateException e)
        {
            return Error.Unauthorized("Proposal.InvalidUpdate", e.Message);
        }

        return response;
    }
}