using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;

namespace ProposalApi.Proposal.UpsertProposal;

public record UpsertProposalCommand(Guid ClientId) : ICommand<ErrorOr<Models.Proposal>>;

public class UpsertProposalCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpsertProposalCommand, ErrorOr<Models.Proposal>>
{
    public async Task<ErrorOr<Models.Proposal>> Handle(UpsertProposalCommand command,
        CancellationToken cancellationToken)
    {
        var newProposal = Models.Proposal.Create(command.ClientId);
        await unitOfWork.Context.AddAsync(newProposal, cancellationToken);
        return newProposal;
    }
}