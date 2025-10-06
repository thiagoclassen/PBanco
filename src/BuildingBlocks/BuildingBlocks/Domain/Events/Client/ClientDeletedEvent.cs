namespace BuildingBlocks.Domain.Events.Client;

public class ClientDeletedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public required Guid ClientId { get; init; }
    public string EventName => nameof(ClientDeletedEvent);
    public DateTime OccurredOn { get; init; }
}