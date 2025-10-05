using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Client;

public class ClientDeletedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ClientDeletedEvent);
    public required Guid ClientId { get; init; }
    public DateTime OccurredOn { get; init; }
}