namespace BuildingBlocks.Outbox.Models;

public class OutboxMessage
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Message { get; set; }
    public required DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; } = "";
}