namespace BuildingBlocks.Domain.Events.Proposal;

public class ProposalPropagateEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(ProposalPropagateEvent);
    public Guid ProposalId { get; init; }
    public required Guid ProposalCreditCardId { get; init; }
    public required string ProposalStatus { get; init; }
    public required decimal ProposalApprovedAmount { get; init; }
    public required string ProposalApprovedCurrency { get; init; }
    public DateTime ProposalRequested { get; init; }
    public DateTime ProposalCreatedAt { get; init; }
    public DateTime ProposalUpdatedAt { get; init; }
    public required DateTime OccurredOn { get; init; }
}