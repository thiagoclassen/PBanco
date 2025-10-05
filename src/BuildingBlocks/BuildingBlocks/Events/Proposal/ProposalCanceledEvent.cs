using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Events.Proposal;

public class ProposalCanceledEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalCanceledEvent);
    public required Guid ProposalId { get; init; }
    public required Guid ClientId { get; init; }
    public DateTime OccurredOn { get; init; }
}