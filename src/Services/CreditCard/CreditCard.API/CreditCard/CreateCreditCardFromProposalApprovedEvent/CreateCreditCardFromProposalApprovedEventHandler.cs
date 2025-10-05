using BuildingBlocks.CQRS;
using BuildingBlocks.Outbox;
using BuildingBlocks.UnitOfWork;
using CreditCard.API.CreditCard.Models;
using CreditCard.API.Data;

namespace CreditCard.API.CreditCard.CreateCreditCardFromProposalApprovedEvent;

public record CreateCreditCardFromProposalApprovedCommand(Guid ProposalId, Guid ClientId, int ApprovedAmount) : ICommand<CreateCreditCardFromProposalApprovedResponse>;

public record CreateCreditCardFromProposalApprovedResponse(Models.CreditCard CreditCard);

public class CreateCreditCardFromProposalApprovedEventCommandHandler(
    IUnitOfWork unitOfWork)

    : ICommandHandler<CreateCreditCardFromProposalApprovedCommand, CreateCreditCardFromProposalApprovedResponse>
{
    public async Task<CreateCreditCardFromProposalApprovedResponse> Handle(CreateCreditCardFromProposalApprovedCommand command, CancellationToken cancellationToken)
    {
        var card = Models.CreditCard.Create(
            clientId: command.ClientId,
            proposalId: command.ProposalId,
            expensesLimit: command.ApprovedAmount,
            cardProvider: CardProvider.Visa
        );
        
        await unitOfWork.Context.AddAsync(card, cancellationToken);
        return new CreateCreditCardFromProposalApprovedResponse(card);
    }
}