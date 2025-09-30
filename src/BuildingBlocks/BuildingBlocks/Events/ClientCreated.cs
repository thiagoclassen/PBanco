namespace BuildingBlocks.Events;

public class ClientCreated
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public Guid ClientId { get; init; }
}