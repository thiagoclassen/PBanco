using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.CancelProposal;

public class CancelProposalEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/proposals/{id:guid}/cancel")]
    public async Task<IActionResult> CancelProposal(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelProposalCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}