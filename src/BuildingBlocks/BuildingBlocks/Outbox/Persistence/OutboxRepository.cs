using BuildingBlocks.Outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Outbox.Persistence;

public class OutboxRepository<TContext> : IOutboxRepository
where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<OutboxMessage> _dbSet;

    public OutboxRepository(TContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<OutboxMessage>();
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken)
    {
        const string sql = $"select * from messaging.OutboxMessages WITH (UPDLOCK, ROWLOCK) WHERE ProcessedOn IS NULL;";
        return await _dbSet.FromSqlRaw(sql).ToListAsync(cancellationToken);
    }

    public void MarkAsProcessed(List<OutboxMessage> messages)
    {
        _dbSet.UpdateRange(messages);
    }
}