using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Client;

public class ClientCreated : IDomainEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public Guid ClientId { get; set; }
}