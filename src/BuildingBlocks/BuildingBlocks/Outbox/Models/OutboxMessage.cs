namespace BuildingBlocks.Outbox.Models;

public class OutboxMessage
{
    public int Id { get; init; }
    public required string Type { get; init; }
    public required string Message { get; init; }
    public required DateTime OccurredOn { get; init; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; init; } = "";
}