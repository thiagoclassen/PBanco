using System.Text.Json;
using BuildingBlocks.Events.Client;
using BuildingBlocks.Events.Proposal;

namespace BuildingBlocks.Events;

public static class EventMapper
{
    public static object? GetConcreteType(string eventTypeName, string message)
    {
        return eventTypeName switch
        {
            _ when eventTypeName == typeof(ClientCreatedEvent).FullName => JsonSerializer.Deserialize<ClientCreatedEvent>(message),
            _ when eventTypeName == typeof(ProposalStatusChangedEvent).FullName => JsonSerializer.Deserialize<ProposalStatusChangedEvent>(message),
            _ => null
        };
    }
}