using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReadService.API.Features.Clients.GetClientById;

public class GetClientByIdEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/clients/{clientId:guid}")]
    public async Task<IActionResult> GetClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
    {
        var query = new GetClientByIdQuery(clientId);
        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            _ => result.Value is null ? NotFound() : Ok(result.Value),
            errors => Problem(errors));
    }
}