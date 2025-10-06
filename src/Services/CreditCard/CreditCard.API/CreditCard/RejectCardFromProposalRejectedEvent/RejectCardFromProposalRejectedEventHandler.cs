using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using MediatR;

namespace CreditCard.API.CreditCard.RejectCardFromProposalRejectedEvent;

public record RejectCardFromProposalRejectedEventCommand(Guid CreditCardId)
    : ICommand<Unit>;

public class RejectCardFromProposalRejectedEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<RejectCardFromProposalRejectedEventCommand, Unit>
{
    public async Task<Unit> Handle(
        RejectCardFromProposalRejectedEventCommand command, CancellationToken cancellationToken)
    {
        var card = await unitOfWork.Context.FindAsync<Models.CreditCard>([command.CreditCardId], cancellationToken);

        if (card is null) throw new Exception("Card not found");

        card.RejectCard();

        unitOfWork.Context.Update(card);
        return Unit.Value;
    }
}