using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Domain.Events.Proposal;

public class ProposalAmountUpdateEvent : IDomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid ClientId { get; init; }
    public required Money NewAmount { get; init; }
    public required Money OldAmount { get; init; }
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalAmountUpdateEvent);
    public DateTime OccurredOn { get; init; }
}