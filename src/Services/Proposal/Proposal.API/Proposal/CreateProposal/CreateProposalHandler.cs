using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.CreateProposal;

public record CreateProposalCommand(Guid ClientId) : ICommand<ErrorOr<CreateProposalResult>>;

public record CreateProposalResult(
    Guid Id,
    Guid CreditCardId,
    string Status,
    string ApprovedAmount,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class CreateProposalCommandHandler(IProposalRepository repository)
    : ICommandHandler<CreateProposalCommand, ErrorOr<CreateProposalResult>>
{
    public async Task<ErrorOr<CreateProposalResult>> Handle(CreateProposalCommand command,
        CancellationToken cancellationToken)
    {
        var proposal = Models.Proposal.Create(command.ClientId);

        await repository.AddAsync(proposal, cancellationToken);

        return new CreateProposalResult(proposal.Id,
            proposal.CreditCardId,
            proposal.Status.ToString(),
            proposal.ApprovedAmount.ToString(),
            proposal.CreatedAt,
            proposal.UpdatedAt);
        ;
    }
}