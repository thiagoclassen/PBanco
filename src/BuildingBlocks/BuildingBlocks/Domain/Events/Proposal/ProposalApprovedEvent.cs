using BuildingBlocks.Domain.Shared;

namespace BuildingBlocks.Domain.Events.Proposal;

public class ProposalApprovedEvent : IDomainEvent
{
    public required Guid ProposalId { get; init; }
    public required Guid CreditCardId { get; init; }
    public required Money ApprovedAmount { get; init; }
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalApprovedEvent);
    public DateTime OccurredOn { get; init; }
}