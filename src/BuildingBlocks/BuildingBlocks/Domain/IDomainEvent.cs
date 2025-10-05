namespace BuildingBlocks.Domain;

public interface IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName { get; }
    public DateTime OccurredOn { get; init; }
}