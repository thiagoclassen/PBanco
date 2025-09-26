using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Client.UpdateClient;

public class UpdateClientEndpoint(ISender sender) : ApiController
{
    [HttpPut("/api/clients/{clientId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid clientId,
        [FromBody] UpdateClientRequest updateClientRequest,
        CancellationToken cancellationToken)
    {
        var command = updateClientRequest.MapToUpdateClientCommand(clientId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors));
    }
}