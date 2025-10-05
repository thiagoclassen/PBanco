using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Client;

public class ClientCanceledEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ClientCanceledEvent);
    public required Guid ClientId { get; init; }
    public DateTime OccurredOn { get; init; }
}