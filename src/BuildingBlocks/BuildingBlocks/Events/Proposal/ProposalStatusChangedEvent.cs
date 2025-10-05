using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Proposal;

public class ProposalStatusChangedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(ProposalStatusChangedEvent);
    public required Guid ProposalId { get; init; }
    public required string NewStatus { get; init; }
    public required string OldStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}