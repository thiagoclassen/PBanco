using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Client.CreateClient;

public class CreateClientEndpoint(ISender sender) : ApiController
{
    [HttpPost("/api/clients")]
    public async Task<IActionResult> CreateClient(
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateClientCommand(request.Name, request.Email, request.CPF, request.BirthDate);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            client => Created($"/api/clients/{client.ClientId}", client),
            errors => Problem(errors)
        );
    }
}