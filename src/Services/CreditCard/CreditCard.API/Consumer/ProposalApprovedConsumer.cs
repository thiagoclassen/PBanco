using BuildingBlocks.Events.Proposal;
using CreditCard.API.CreditCard.CreateCreditCardFromProposalApprovedEvent;
using MassTransit;
using MediatR;

namespace CreditCard.API.Consumer;

public class ProposalApprovedConsumer(
    ISender sender,
    ILogger<ProposalApprovedConsumer> logger) : IConsumer<ProposalApprovedEvent>
{
    public async Task Consume(ConsumeContext<ProposalApprovedEvent> context)
    {
        var command = new CreateCreditCardFromProposalApprovedCommand(
            context.Message.ProposalId,
            context.Message.ClientId,
            context.Message.ApprovedAmount);

        await sender.Send(command, context.CancellationToken);
    }
}