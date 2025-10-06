namespace BuildingBlocks.Domain.Events.Proposal;

public class ProposalRejectedEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public string EventName => nameof(ProposalRejectedEvent);
    public required Guid ProposalId { get; init; }
    public required Guid CreditCardId { get; init; }
    public DateTime OccurredOn { get; init; }
}