using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Client.DeleteClient;

public class DeleteClientEndpoint(ISender sender) : ApiController
{
    [HttpDelete("api/clients/{clientId:guid}")]
    public async Task<IActionResult> DeleteClientById([FromRoute] Guid clientId, CancellationToken cancellationToken)
    {
        var command = new DeleteClientCommand(clientId);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => result.Value.Success ? NoContent() : NotFound(),
            errors => Problem(errors)
        );
    }
}