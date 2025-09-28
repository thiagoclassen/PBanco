namespace Clients.API.Client.Events;

public record ClientCreated
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public Guid ClientId { get; init; }
}