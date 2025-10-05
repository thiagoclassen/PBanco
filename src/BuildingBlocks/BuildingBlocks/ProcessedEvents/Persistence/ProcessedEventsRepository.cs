using BuildingBlocks.ProcessedEvents.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.ProcessedEvents.Persistence;

public class ProcessedEventsRepository<TContext>(TContext dbContext) : IProcessedEventsRepository
    where TContext : DbContext
{
    private readonly DbSet<ProcessedEvent> _dbSet = dbContext.Set<ProcessedEvent>();

    public async Task AddAsync(ProcessedEvent processedEvent, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(processedEvent, cancellationToken);
        //await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync([eventId], cancellationToken) is not null;
    }
}