using BuildingBlocks.Events.Proposal;
using MassTransit;
using ReadService.API.Features.Proposals.Models;
using ReadService.API.Features.Proposals.Repository;
using ReadService.API.Features.Shared;

namespace ReadService.API.Features.Proposals.Consumers;

public class ProposalPropagateEventConsumer(IProposalRepository repository) : IConsumer<ProposalPropagateEvent>
{
    public async Task Consume(ConsumeContext<ProposalPropagateEvent> context)
    {
        var @event = context.Message;

        var proposalDocument = new ProposalDocument
        {
            Id = @event.ProposalId,
            ClientId = @event.ProposalClientId,
            ProposalStatus = @event.ProposalStatus,
            ApprovedAmount = new MoneyDocument(@event.ProposalApprovedAmount, @event.ProposalApprovedCurrency),
            Requested = @event.ProposalRequested,
            CreatedAt = @event.ProposalCreatedAt,
            UpdatedAt = @event.ProposalUpdatedAt
        };

        await repository.InsertOrUpdateAsync(proposalDocument);
    }
}