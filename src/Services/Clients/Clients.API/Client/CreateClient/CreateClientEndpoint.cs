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
        var command = request.MapToCreateClientCommand();
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Created($"/api/clients/{result.Value.Id}", result.Value.MapToCreateClientResult()),
            errors => Problem(errors)
        );
    }
}