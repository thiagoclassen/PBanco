using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API.CreditCard.CancelCreditCardsFromClientCanceledEvent;

public record CancelCreditCardsFromClientCanceledEventCommand(Guid ClientId)
    : ICommand<CancelCreditCardsFromClientCanceledEventEventResponse>;

public record CancelCreditCardsFromClientCanceledEventEventResponse;

public class CancelCreditCardsFromClientCanceledEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelCreditCardsFromClientCanceledEventCommand,
        CancelCreditCardsFromClientCanceledEventEventResponse>
{
    public async Task<CancelCreditCardsFromClientCanceledEventEventResponse> Handle(
        CancelCreditCardsFromClientCanceledEventCommand request, CancellationToken cancellationToken)
    {
        var cards = await unitOfWork.Context.Set<Models.CreditCard>()
            .Where(c => c.ClientId == request.ClientId)
            .ToListAsync(cancellationToken);

        if (cards is null || !cards.Any())
            throw new Exception("Card not found");

        cards.ForEach(c => c.Cancel());

        unitOfWork.Context.UpdateRange(cards);

        return new CancelCreditCardsFromClientCanceledEventEventResponse();
    }
}