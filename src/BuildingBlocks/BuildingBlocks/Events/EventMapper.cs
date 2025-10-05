using System.Text.Json;
using BuildingBlocks.Events.Client;
using BuildingBlocks.Events.CreditCard;
using BuildingBlocks.Events.Proposal;

namespace BuildingBlocks.Events;

public static class EventMapper
{
    public static object? GetConcreteType(string eventTypeName, string message)
    {
        return eventTypeName switch
        {
            // Client events
            _ when eventTypeName == typeof(ClientCreatedEvent).FullName => JsonSerializer.Deserialize<ClientCreatedEvent>(message),
            _ when eventTypeName == typeof(ClientCanceledEvent).FullName => JsonSerializer.Deserialize<ClientCanceledEvent>(message),
            // Proposal events
            _ when eventTypeName == typeof(ProposalAmountUpdateEvent).FullName => JsonSerializer.Deserialize<ProposalAmountUpdateEvent>(message),
            _ when eventTypeName == typeof(ProposalApprovedEvent).FullName => JsonSerializer.Deserialize<ProposalApprovedEvent>(message),
            _ when eventTypeName == typeof(ProposalCanceledEvent).FullName => JsonSerializer.Deserialize<ProposalCanceledEvent>(message),
            _ when eventTypeName == typeof(ProposalCreatedEvent).FullName => JsonSerializer.Deserialize<ProposalCreatedEvent>(message),
            _ when eventTypeName == typeof(ProposalRejectedEvent).FullName => JsonSerializer.Deserialize<ProposalRejectedEvent>(message),
            _ when eventTypeName == typeof(ProposalStatusChangedEvent).FullName => JsonSerializer.Deserialize<ProposalStatusChangedEvent>(message),
            // CreditCard events
            _ when eventTypeName == typeof(CreditCardActiveEvent).FullName => JsonSerializer.Deserialize<CreditCardActiveEvent>(message),
            _ when eventTypeName == typeof(CreditCardActiveEvent).FullName => JsonSerializer.Deserialize<CreditCardActiveEvent>(message),
            _ when eventTypeName == typeof(CreditCardBlockedEvent).FullName => JsonSerializer.Deserialize<CreditCardBlockedEvent>(message),
            _ when eventTypeName == typeof(CreditCardCreatedEvent).FullName => JsonSerializer.Deserialize<CreditCardCreatedEvent>(message),
            _ when eventTypeName == typeof(CreditCardStatusChangedEvent).FullName => JsonSerializer.Deserialize<CreditCardStatusChangedEvent>(message),
            _ => null
        };
    }
}