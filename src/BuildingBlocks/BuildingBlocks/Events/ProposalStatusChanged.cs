namespace BuildingBlocks.Events;

public class ProposalStatusChanged
{
    public int Id { get; set; }
    public Guid ProposalId { get; set; }
    public required string NewStatus { get; set; }
    public DateTime Created { get; set; }
}