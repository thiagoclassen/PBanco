using BuildingBlocks.Domain.Events.CreditCard;
using MassTransit;
using MediatR;
using ProposalApi.Proposal.CreateProposalFromCreditCardCreatedEvent;

namespace ProposalApi.Consumer;

public class CreditCardCreatedConsumer(
    ISender sender) : IConsumer<CreditCardCreatedEvent>
{
    public async Task Consume(ConsumeContext<CreditCardCreatedEvent> context)
    {
        var command = new CreateProposalFromCreditCardCreatedEventCommand(context.Message.CreditCardId);
        await sender.Send(command);
    }
}
