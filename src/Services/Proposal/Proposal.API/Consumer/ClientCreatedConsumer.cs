using BuildingBlocks.Events.Client;
using MassTransit;
using MediatR;
using ProposalApi.Proposal.CreateProposal;

namespace ProposalApi.Consumer;

public class ClientCreatedConsumer(ISender sender, ILogger<ClientCreatedConsumer> logger) : IConsumer<ClientCreatedEvent>
{
    public async Task Consume(ConsumeContext<ClientCreatedEvent> context)
    {
        var command = new CreateProposalCommand(context.Message.ClientId);
        var response = await sender.Send(command);
    }
}