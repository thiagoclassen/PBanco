using Clients.API.Outbox.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Clients.API.Data;

public class UnitOfWork(
    ClientDbContext context,
    IOutboxRepository outboxRepository) : IDisposable
{
    private IDbContextTransaction? _transaction;
    public IOutboxRepository Outbox => outboxRepository;

    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
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
}