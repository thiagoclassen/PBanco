using BuildingBlocks.Outbox.Models;
using Clients.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Clients.API.Outbox.Persistence;

public class OutboxRepository(ClientDbContext context) : IOutboxRepository
{
    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken)
    {
        const string sql = $"select * from messaging.OutboxMessages WITH (UPDLOCK, ROWLOCK) WHERE ProcessedOn IS NULL;";
        return await context.OutboxMessages.FromSqlRaw(sql).ToListAsync(cancellationToken);
        // return await context.OutboxMessages
        //     .Where(m => m.ProcessedOn == null)
        //     .ToListAsync(cancellationToken: cancellationToken);
    }

    public void MarkAsProcessed(List<OutboxMessage> messages)
    {
        context.OutboxMessages.UpdateRange(messages);
    }
}