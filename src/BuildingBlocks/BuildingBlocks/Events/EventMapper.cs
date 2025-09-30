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
            _ when eventTypeName == typeof(ClientCreated).FullName => JsonSerializer.Deserialize<ClientCreated>(message),
            _ when eventTypeName == typeof(ProposalStatusChanged).FullName => JsonSerializer.Deserialize<ProposalStatusChanged>(message),
            _ => null
        };
    }
}