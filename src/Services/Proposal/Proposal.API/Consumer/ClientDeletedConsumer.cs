using BuildingBlocks.Events.Client;
using MassTransit;
using MediatR;
using ProposalApi.Proposal.CancelProposalByClientDeletedEvent;

namespace ProposalApi.Consumer;

public class ClientDeletedConsumer(
    ISender sender) : IConsumer<ClientDeletedEvent>
{
    public async Task Consume(ConsumeContext<ClientDeletedEvent> context)
    {
        var command = new CancelProposalByClientDeletedEventCommand(context.Message.ClientId);
        await sender.Send(command);
    }
}