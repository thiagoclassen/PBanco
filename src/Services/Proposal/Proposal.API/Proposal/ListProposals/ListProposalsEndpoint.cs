using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.ListProposals;

public class ListProposalsEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/proposals")]
    public async Task<IActionResult> ListProposals([FromQuery]Guid? userId,CancellationToken cancellationToken)
    {
        var command = new ListProposalsQuery(userId);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(result.Value.Select(p => p.MapToProposalResponse())),
            errors => Problem(errors)
        );
    }
}