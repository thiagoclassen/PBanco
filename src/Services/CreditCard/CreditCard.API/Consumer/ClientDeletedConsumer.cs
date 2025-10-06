using BuildingBlocks.Domain.Events.Client;
using CreditCard.API.CreditCard.CancelCreditCardsFromClientCanceledEvent;
using MassTransit;
using MediatR;

namespace CreditCard.API.Consumer;

public class ClientDeletedConsumer(
    ISender sender) : IConsumer<ClientDeletedEvent>
{
    public async Task Consume(ConsumeContext<ClientDeletedEvent> context)
    {
        var command = new CancelCreditCardsFromClientCanceledEventCommand(context.Message.ClientId);

        await sender.Send(command, context.CancellationToken);
    }
}