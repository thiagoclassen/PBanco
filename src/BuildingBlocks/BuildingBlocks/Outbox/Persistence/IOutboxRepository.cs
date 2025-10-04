using BuildingBlocks.Outbox.Models;

namespace BuildingBlocks.Outbox.Persistence;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken);
    void MarkAsProcessed(List<OutboxMessage> messages);
}