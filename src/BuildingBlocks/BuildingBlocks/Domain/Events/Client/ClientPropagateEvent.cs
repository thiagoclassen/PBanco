namespace BuildingBlocks.Domain.Events.Client;

public class ClientPropagateEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(ClientPropagateEvent);

    public required Guid ClientId { get; init; }
    public required string ClientName { get; init; }
    public required string ClientEmail { get; init; }
    public required string ClientCPF { get; init; }
    public required DateTime ClientBirthDate { get; init; }
    public required bool ClientIsDeleted { get; init; }
    public required DateTime? ClientDeletedAt { get; init; }
    public required DateTime ClientCreatedAt { get; init; }
    public required DateTime ClientUpdatedAt { get; init; }
    public required DateTime OccurredOn { get; init; }
}