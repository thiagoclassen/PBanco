using Clients.API.Client.Events;
using MassTransit;

namespace Clients.API.Messages;

public class MessageService(IPublishEndpoint publishEndpoint)
{
    public async Task PublishClientCreated(Guid clientId)
    {
        await publishEndpoint.Publish(new ClientCreated
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            ClientId = clientId
        });
    }
}