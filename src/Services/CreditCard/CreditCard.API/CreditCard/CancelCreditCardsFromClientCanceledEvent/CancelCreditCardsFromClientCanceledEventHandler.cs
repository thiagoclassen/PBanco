using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using CreditCard.API.CreditCard.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API.CreditCard.CancelCreditCardsFromClientCanceledEvent;

public record CancelCreditCardsFromClientCanceledEventCommand(Guid ClientId)
    : ICommand<Unit>;

public class CancelCreditCardsFromClientCanceledEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelCreditCardsFromClientCanceledEventCommand, Unit>
{
    public async Task<Unit> Handle(
        CancelCreditCardsFromClientCanceledEventCommand request, CancellationToken cancellationToken)
    {
        var cards = await unitOfWork.Context.Set<Models.CreditCard>()
            .Where(c => c.ClientId == request.ClientId && c.Status != CardStatus.Rejected &&
                        c.Status != CardStatus.Canceled)
            .ToListAsync(cancellationToken);

        if (cards is null || !cards.Any())
            throw new Exception("Card not found");

        cards.ForEach(c => c.Cancel());

        unitOfWork.Context.UpdateRange(cards);

        return Unit.Value;
    }
}