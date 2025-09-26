using Clients.API.Client.CreateClient;
using Clients.API.Client.GetClient;
using Clients.API.Client.UpdateClient;
using Clients.API.Models;

namespace Clients.API.Client;

public static class ClientMapping
{
    public static CreateClientCommand MapToCreateClientCommand(this CreateClientRequest request)
    {
        return new CreateClientCommand(request.Name, request.Email, request.CPF, request.BirthDate);
    }
    
    public static CreateClientResult MapToCreateClientResult(this BankClient client)
    {
        return new CreateClientResult(client.Id);
    }
    
    public static BankClient MapToBankClient(this CreateClientCommand command)
    {
        return new BankClient
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            CPF = command.CPF,
            BirthDate = command.BirthDate
        };
    }

    public static GetClientResult MapToClientResult(this BankClient client)
    {
        return new GetClientResult(
            client.Id,
            client.Name,
            client.Email,
            client.CPF,
            client.BirthDate,
            client.CreatedAt,
            client.UpdatedAt);
    }

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