using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clients.API.Client.GetClient;

public class GetClientEndpoint(ISender sender) : ApiController
{
    //[HttpGet("/api/clients/{clientId:guid}")]
    // public async Task<IActionResult> GetClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
    // {
    //     var query = new GetClientQuery(clientId);
    //     var result = await sender.Send(query, cancellationToken);
    //
    //     return result.Match(
    //         _ => result.Value is null ? NotFound() : Ok(result.Value.MapToClientResult()),
    //         errors => Problem(errors));
    // }
}