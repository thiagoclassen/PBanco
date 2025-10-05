using BuildingBlocks.Events.Client;
using MassTransit;
using MediatR;
using ProposalApi.Proposal.UpsertProposal;

namespace ProposalApi.Consumer;

public class ClientCreatedConsumer(
    ISender sender,
    ILogger<ClientCreatedConsumer> logger) : IConsumer<ClientCreatedEvent>
{
    public async Task Consume(ConsumeContext<ClientCreatedEvent> context)
    {
        var command = new UpsertProposalCommand(context.Message.ClientId);
        var response = await sender.Send(command);
    }
}