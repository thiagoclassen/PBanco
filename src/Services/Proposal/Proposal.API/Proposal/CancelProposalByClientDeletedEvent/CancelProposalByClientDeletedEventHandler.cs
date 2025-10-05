using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ProposalApi.Proposal.CancelProposalByClientDeletedEvent;

public record CancelProposalByClientDeletedEventCommand(Guid ClientId)
    : ICommand<CancelProposalByClientDeletedEventResponse>;

public record CancelProposalByClientDeletedEventResponse;

public class CancelProposalByClientDeletedCommandEventHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CancelProposalByClientDeletedEventCommand, CancelProposalByClientDeletedEventResponse>
{
    public async Task<CancelProposalByClientDeletedEventResponse> Handle(
        CancelProposalByClientDeletedEventCommand command, CancellationToken cancellationToken)
    {
        var proposals = await unitOfWork.Context.Set<Models.Proposal>()
            .Where(p => p.ClientId == command.ClientId && p.ProposalStatus != Models.ProposalStatus.Canceled)
            .ToListAsync(cancellationToken);

        foreach (var proposal in proposals)
        {
            proposal.CancelWithoutEvent();
        }

        unitOfWork.Context.UpdateRange(proposals);

        return new CancelProposalByClientDeletedEventResponse();
    }
}