using System.Text.Json;
using BuildingBlocks.Domain.Events.Client;
using BuildingBlocks.Domain.Events.CreditCard;
using BuildingBlocks.Domain.Events.Proposal;

namespace BuildingBlocks.Domain.Events;

public static class EventMapper
{
    public static object? GetConcreteType(string eventTypeName, string message)
    {
        return eventTypeName switch
        {
            // Client events
            _ when eventTypeName == typeof(ClientCreatedEvent).FullName => JsonSerializer.Deserialize<ClientCreatedEvent>(message),
            _ when eventTypeName == typeof(ClientDeletedEvent).FullName => JsonSerializer.Deserialize<ClientDeletedEvent>(message),
            _ when eventTypeName == typeof(ClientPropagateEvent).FullName => JsonSerializer.Deserialize<ClientPropagateEvent>(message),
            // Proposal events
            _ when eventTypeName == typeof(ProposalAmountUpdateEvent).FullName => JsonSerializer.Deserialize<ProposalAmountUpdateEvent>(message),
            _ when eventTypeName == typeof(ProposalApprovedEvent).FullName => JsonSerializer.Deserialize<ProposalApprovedEvent>(message),
            _ when eventTypeName == typeof(ProposalCreatedEvent).FullName => JsonSerializer.Deserialize<ProposalCreatedEvent>(message),
            _ when eventTypeName == typeof(ProposalPropagateEvent).FullName => JsonSerializer.Deserialize<ProposalPropagateEvent>(message),
            _ when eventTypeName == typeof(ProposalRejectedEvent).FullName => JsonSerializer.Deserialize<ProposalRejectedEvent>(message),
            // CreditCard events
            _ when eventTypeName == typeof(CreditCardActiveEvent).FullName => JsonSerializer.Deserialize<CreditCardActiveEvent>(message),
            _ when eventTypeName == typeof(CreditCardBlockedEvent).FullName => JsonSerializer.Deserialize<CreditCardBlockedEvent>(message),
            _ when eventTypeName == typeof(CreditCardCanceledEvent).FullName => JsonSerializer.Deserialize<CreditCardCanceledEvent>(message),
            _ when eventTypeName == typeof(CreditCardCreatedEvent).FullName => JsonSerializer.Deserialize<CreditCardCreatedEvent>(message),
            _ when eventTypeName == typeof(CreditCardPropagateEvent).FullName => JsonSerializer.Deserialize<CreditCardPropagateEvent>(message),
            _ => null
        };
    }
}