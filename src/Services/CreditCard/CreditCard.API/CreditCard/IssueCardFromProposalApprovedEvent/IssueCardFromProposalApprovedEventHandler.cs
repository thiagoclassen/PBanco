using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using BuildingBlocks.UnitOfWork;
using MediatR;

namespace CreditCard.API.CreditCard.IssueCardFromProposalApprovedEvent;

public record IssueCardFromProposalApprovedEventCommand(Guid ProposalId, Guid CreditCardId, Money ApprovedAmount)
    : ICommand<Unit>;

public class IssueCardFromProposalApprovedEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<IssueCardFromProposalApprovedEventCommand, Unit>
{
    public async Task<Unit> Handle(
        IssueCardFromProposalApprovedEventCommand command, CancellationToken cancellationToken)
    {
        var card = await unitOfWork.Context.FindAsync<Models.CreditCard>([command.CreditCardId], cancellationToken);

        if(card is null) throw new Exception("Card not found");
        
        card.IssueCardSetInactive(command.ApprovedAmount);

        unitOfWork.Context.Update(card);
        return Unit.Value;
    }
}