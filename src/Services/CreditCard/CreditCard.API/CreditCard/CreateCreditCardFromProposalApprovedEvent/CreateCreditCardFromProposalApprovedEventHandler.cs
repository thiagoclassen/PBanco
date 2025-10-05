using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using BuildingBlocks.UnitOfWork;
using CreditCard.API.CreditCard.Models;

namespace CreditCard.API.CreditCard.CreateCreditCardFromProposalApprovedEvent;

public record CreateCreditCardFromProposalApprovedCommand(Guid ProposalId, Guid ClientId, Money ApprovedAmount)
    : ICommand<CreateCreditCardFromProposalApprovedResponse>;

public record CreateCreditCardFromProposalApprovedResponse(Models.CreditCard CreditCard);

public class CreateCreditCardFromProposalApprovedEventCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCreditCardFromProposalApprovedCommand, CreateCreditCardFromProposalApprovedResponse>
{
    public async Task<CreateCreditCardFromProposalApprovedResponse> Handle(
        CreateCreditCardFromProposalApprovedCommand command, CancellationToken cancellationToken)
    {
        var card = Models.CreditCard.Create(
            command.ClientId,
            command.ProposalId,
            command.ApprovedAmount,
            CardProvider.Visa
        );

        await unitOfWork.Context.AddAsync(card, cancellationToken);
        return new CreateCreditCardFromProposalApprovedResponse(card);
    }
}