using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReadService.API.Features.Proposals.ListProposals;

public class ListProposalsEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/proposals")]
    public async Task<IActionResult> ListProposals([FromQuery] Guid? clientId, CancellationToken cancellationToken)
    {
        var command = new ListProposalsQuery(clientId);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors)
        );
    }
}