using BuildingBlocks.ProcessedEvents.Models;

namespace BuildingBlocks.ProcessedEvents.Persistence;

public interface IProcessedEventsRepository
{
    Task AddAsync(ProcessedEvent processedEvent, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken);
}