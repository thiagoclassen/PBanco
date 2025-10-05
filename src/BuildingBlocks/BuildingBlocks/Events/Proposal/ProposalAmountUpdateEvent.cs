using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Events.Proposal;

public class ProposalAmountUpdateEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalAmountUpdateEvent);
    public Guid ProposalId { get; init; }
    public Guid ClientId { get; init; }
    public required Money NewAmount { get; init; }
    public required Money OldAmount { get; init; }
    public DateTime OccurredOn { get; init; }
}