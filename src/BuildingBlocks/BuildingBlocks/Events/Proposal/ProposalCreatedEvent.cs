using BuildingBlocks.Domain;

namespace BuildingBlocks.Events.Proposal;

public class ProposalCreatedEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    
    public required Guid ProposalId { get; init; }
    public required Guid ClientId { get; init; }
    public required string ProposalStatus { get; init; }
}