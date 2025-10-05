using BuildingBlocks.Events.Proposal;
using CreditCard.API.CreditCard.CancelCreditCardsFromProposalCanceledEvent;
using MassTransit;
using MediatR;

namespace CreditCard.API.Consumer;

public class ProposalCanceledConsumer(
    ISender sender) : IConsumer<ProposalCanceledEvent>
{
    public async Task Consume(ConsumeContext<ProposalCanceledEvent> context)
    {
        var command = new CancelCreditCardFromProposalCanceledEventCommand(
            context.Message.ProposalId,
            context.Message.ClientId);

        await sender.Send(command, context.CancellationToken);
    }
}