using BuildingBlocks.Domain.Events.Client;
using MassTransit;
using MediatR;
using ProposalApi.Proposal.RejectPendingProposalsFromCanceledClient;

namespace ProposalApi.Consumer;

public class ClientCanceledConsumer(
    ISender sender) : IConsumer<ClientDeletedEvent>
{
    public async Task Consume(ConsumeContext<ClientDeletedEvent> context)
    {
        var command = new RejectPendingProposalsFromCanceledClientCommand(context.Message.ClientId);
        await sender.Send(command);
    }
}
