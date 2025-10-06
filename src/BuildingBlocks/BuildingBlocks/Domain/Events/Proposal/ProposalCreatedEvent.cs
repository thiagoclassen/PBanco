namespace BuildingBlocks.Domain.Events.Proposal;

public class ProposalCreatedEvent : IDomainEvent
{
    public required Guid EventId { get; init; }
    public string EventName => nameof(ProposalCreatedEvent);
    public required Guid ProposalId { get; init; }
    public required Guid CreditCardId { get; init; }
    public required string ProposalStatus { get; init; }
    public required DateTime OccurredOn { get; init; }
}