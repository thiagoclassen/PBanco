using BuildingBlocks.Outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Outbox.Persistence;

public class OutboxRepository<TContext>(TContext dbContext) : IOutboxRepository
    where TContext : DbContext
{
    private readonly DbSet<OutboxMessage> _dbSet = dbContext.Set<OutboxMessage>();

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken)
    {
        const string sql =
            "select * from messaging.OutboxMessages WITH (UPDLOCK,ROWLOCK,READPAST) WHERE ProcessedOn IS NULL;";
        return await _dbSet.FromSqlRaw(sql).ToListAsync(cancellationToken);
    }

    public void MarkAsProcessed(List<OutboxMessage> messages)
    {
        _dbSet.UpdateRange(messages);
    }
}