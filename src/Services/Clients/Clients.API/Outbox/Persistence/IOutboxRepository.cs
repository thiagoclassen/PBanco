using Clients.API.Outbox.Models;

namespace Clients.API.Outbox.Persistence;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken);
    void MarkAsProcessed(List<OutboxMessage> messages);
}