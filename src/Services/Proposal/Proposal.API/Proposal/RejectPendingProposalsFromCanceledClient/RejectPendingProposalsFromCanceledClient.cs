using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using MediatR;

namespace ProposalApi.Proposal.RejectPendingProposalsFromCanceledClient;

public record RejectPendingProposalsFromCanceledClientCommand(Guid ClientId) : ICommand<Unit>;

public class
    RejectPendingProposalsFromCanceledClient(IUnitOfWork unitOfWork)
    : ICommandHandler<RejectPendingProposalsFromCanceledClientCommand, Unit>
{
    public async Task<Unit> Handle(RejectPendingProposalsFromCanceledClientCommand request,
        CancellationToken cancellationToken)
    {
        // TODO - gRPC into credit card service to get all proposals by client id
        return Unit.Value;
    }
}