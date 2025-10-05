using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API.CreditCard.CancelCreditCardsFromProposalCanceledEvent;

public record CancelCreditCardFromProposalCanceledEventCommand(Guid ProposalId, Guid ClientId)
    : ICommand<CancelCreditCardFromProposalCanceledEventResponse>;

public record CancelCreditCardFromProposalCanceledEventResponse();

public class CancelCreditCardFromProposalCanceledEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelCreditCardFromProposalCanceledEventCommand,
        CancelCreditCardFromProposalCanceledEventResponse>
{
    public async Task<CancelCreditCardFromProposalCanceledEventResponse> Handle(
        CancelCreditCardFromProposalCanceledEventCommand request, CancellationToken cancellationToken)
    {
        var cards = await unitOfWork.Context.Set<Models.CreditCard>()
            .Where(c => c.ProposalId == request.ProposalId && c.ClientId == request.ClientId)
            .ToListAsync(cancellationToken);

        if (cards is null || !cards.Any())
            throw new Exception("Card not found");

        cards.ForEach(c => c.Cancel());

        unitOfWork.Context.UpdateRange(cards);

        return new CancelCreditCardFromProposalCanceledEventResponse();
    }
}