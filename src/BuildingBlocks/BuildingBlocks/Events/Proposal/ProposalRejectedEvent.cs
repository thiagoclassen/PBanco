using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Proposal;

public class ProposalRejectedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalRejectedEvent);
    public required Guid ProposalId { get; init; }
    public required Guid ClientId { get; init; }
    public DateTime OccurredOn { get; init; }
}