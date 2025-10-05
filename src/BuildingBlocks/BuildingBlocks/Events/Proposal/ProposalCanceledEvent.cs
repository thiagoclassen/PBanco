using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Proposal;

public class ProposalCanceledEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalCanceledEvent);
    public required Guid ProposalId { get; init; }
    public required Guid ClientId { get; init; }
    public required int ApprovedAmount { get; init; }
    public DateTime OccurredOn { get; init; }
}