using BuildingBlocks.Domain.Events.Proposal;
using CreditCard.API.CreditCard.RejectCardFromProposalRejectedEvent;
using MassTransit;
using MediatR;

namespace CreditCard.API.Consumer;

public class ProposalRejectedConsumer(
    ISender sender) : IConsumer<ProposalRejectedEvent>
{
    public async Task Consume(ConsumeContext<ProposalRejectedEvent> context)
    {
        var command = new RejectCardFromProposalRejectedEventCommand(context.Message.CreditCardId);

        await sender.Send(command, context.CancellationToken);
    }
}