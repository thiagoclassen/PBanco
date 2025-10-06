using BuildingBlocks.Events.CreditCard;
using MassTransit;
using ReadService.API.Features.ClientCreditCardView.Repository;
using ReadService.API.Features.CreditCards.Models;
using ReadService.API.Features.CreditCards.Repository;
using ReadService.API.Features.Shared;

namespace ReadService.API.Features.CreditCards.Consumers;

public class CreditCardPropagateEventConsumer(
    ICreditCardRepository creditCardRepository,
    IClientCreditCardViewRepository clientCreditCardViewRepository)
    : IConsumer<CreditCardPropagateEvent>
{
    public async Task Consume(ConsumeContext<CreditCardPropagateEvent> context)
    {
        var @event = context.Message;

        var clientDocument = new CreditCardDocument
        {
            Id = @event.CreditCardId,
            ClientId = @event.CreditCardClientId,
            ProposalId = @event.CreditCardProposalId,
            ExpensesLimit =
                new MoneyDocument(@event.CreditCardExpensesLimitAmount, @event.CreditCardExpensesLimitCurrency),
            Number = @event.CreditCardNumber,
            CardStatus = @event.CardStatus,
            CardProvider = @event.CardProvider,
            CreatedAt = @event.CreditCardCreatedAt,
            UpdatedAt = @event.CreditCardUpdatedAt
        };
        
        await creditCardRepository.InsertOrUpdateAsync(clientDocument);
        await clientCreditCardViewRepository.UpsertCreditCardAsync(@event.CreditCardClientId);
    }
}