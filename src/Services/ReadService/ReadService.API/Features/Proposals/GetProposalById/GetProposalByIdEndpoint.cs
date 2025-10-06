using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReadService.API.Features.Proposals.GetProposalById;

public class GetProposalByIdEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/proposals/{proposalId:guid}")]
    public async Task<IActionResult> GetProposal(
        [FromRoute] Guid proposalId,
        CancellationToken cancellationToken)
    {
        var command = new GetProposalByIdQuery(proposalId);
        var result = await sender.Send(command, cancellationToken);
        return result.Match(
            _ => result.Value is null ? NoContent() : Ok(result.Value),
            errors => Problem(errors)
        );
    }
}