using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Proposal;

public class ProposalStatusChanged : IDomainEvent
{
    public Guid Id { get; set; }
    public Guid ProposalId { get; set; }
    public required string NewStatus { get; set; }
    public required string OldStatus { get; set; }
    public DateTime OccurredOn { get; set; }
}