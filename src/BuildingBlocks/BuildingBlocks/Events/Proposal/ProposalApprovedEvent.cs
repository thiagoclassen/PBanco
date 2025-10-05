using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Events.Proposal;

public class ProposalApprovedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName=> nameof(ProposalApprovedEvent);
    public required Guid ProposalId { get; init; }
    public required Guid ClientId { get; init; }
    public required Money ApprovedAmount { get; init; }
    public DateTime OccurredOn { get; init; }
}