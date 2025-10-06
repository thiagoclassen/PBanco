using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using ProposalApi.Proposal.Exceptions;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.ApproveProposal;

public record ApproveProposalCommand(Guid ProposalId, decimal ApprovedAmount, string ApprovedCurrency)
    : ICommand<ErrorOr<ApproveProposalResult>>;

public record ApproveProposalResult(
    Guid Id,
    Guid CreditCardId,
    string Status,
    string ApprovedAmount,
    DateTime UpdatedAt);

public class ApproveProposalCommandHandler(
    IProposalRepository repository) : ICommandHandler<ApproveProposalCommand, ErrorOr<ApproveProposalResult>>
{
    public async Task<ErrorOr<ApproveProposalResult>> Handle(ApproveProposalCommand command,
        CancellationToken cancellationToken)
    {
        Models.Proposal? proposal;
        try
        {
            proposal = await repository.GetByIdAsync(command.ProposalId, cancellationToken);

            if (proposal is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");

            proposal.Approve(new Money(command.ApprovedAmount, command.ApprovedCurrency));

            await repository.UpdateAsync(proposal, cancellationToken);
        }
        catch (InvalidProposalStatusStateException e)
        {
            return Error.Forbidden("Proposal.InvalidState", e.Message);
        }

        return new ApproveProposalResult(proposal.Id, proposal.CreditCardId, proposal.Status.ToString(),
            proposal.ApprovedAmount.ToString(), proposal.UpdatedAt);
    }
}