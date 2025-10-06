using BuildingBlocks.Domain.Events.Proposal;
using CreditCard.API.CreditCard.IssueCardFromProposalApprovedEvent;
using MassTransit;
using MediatR;

namespace CreditCard.API.Consumer;

public class ProposalApprovedConsumer(
    ISender sender) : IConsumer<ProposalApprovedEvent>
{
    public async Task Consume(ConsumeContext<ProposalApprovedEvent> context)
    {
        var command = new IssueCardFromProposalApprovedEventCommand(
            context.Message.ProposalId,
            context.Message.CreditCardId,
            context.Message.ApprovedAmount);

        await sender.Send(command, context.CancellationToken);
    }
}