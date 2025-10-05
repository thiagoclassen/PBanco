using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Client;

public class ClientCreatedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(ClientCreatedEvent);
    public required DateTime OccurredOn { get; init; }
    public required Guid ClientId { get; init; }
}