using Clients.API.Client.Models;
using Clients.API.Client.UpdateClient;

namespace Clients.API.Client;

public static class ClientMapping
{
    public static UpdateClientResponse MapToUpdateClientResponse(this BankClient client)
    {
        return new UpdateClientResponse(
            client.Id,
            client.Name,
            client.Email,
            client.CPF,
            client.BirthDate,
            client.CreatedAt,
            client.UpdatedAt);
    }

    public static UpdateClientCommand MapToUpdateClientCommand(this UpdateClientRequest request, Guid clientId)
    {
        return new UpdateClientCommand(
            clientId,
            request.Name,
            request.Email,
            request.CPF,
            request.BirthDate);
    }
}