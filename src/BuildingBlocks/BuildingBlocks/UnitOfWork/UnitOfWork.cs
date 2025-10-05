using BuildingBlocks.Outbox.Persistence;
using BuildingBlocks.ProcessedEvents.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuildingBlocks.UnitOfWork;

public class UnitOfWork<TContext>(
    TContext context,
    IOutboxRepository outboxRepository,
    IProcessedEventsRepository processedEventsRepository)
    : IUnitOfWork
    where TContext : DbContext

{
    private IDbContextTransaction? _transaction;
    public DbContext Context => context;
    public IOutboxRepository Outbox => outboxRepository;
    public IProcessedEventsRepository ProcessedEvents => processedEventsRepository;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _transaction?.DisposeAsync();
        return context.DisposeAsync();
    }
}