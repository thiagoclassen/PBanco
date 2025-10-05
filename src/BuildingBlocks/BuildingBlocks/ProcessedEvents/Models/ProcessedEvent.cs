namespace BuildingBlocks.ProcessedEvents.Models;

public class ProcessedEvent
{
    public Guid EventId { get; init; }
    public required string EventName { get; init; }
    public required string Message { get; init; }
    public DateTime ProcessedOn { get; init; } = DateTime.UtcNow;
    public DateTime ProcessedAt { get; set; }
}