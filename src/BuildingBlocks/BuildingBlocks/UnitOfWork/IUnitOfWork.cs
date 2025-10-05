using BuildingBlocks.Outbox.Persistence;
using BuildingBlocks.ProcessedEvents.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.UnitOfWork;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    DbContext Context { get; }
    IOutboxRepository Outbox { get; }
    IProcessedEventsRepository ProcessedEvents { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync();
    Task RollbackAsync();
}