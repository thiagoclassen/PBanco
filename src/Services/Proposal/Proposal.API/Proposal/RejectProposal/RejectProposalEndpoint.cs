using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.RejectProposal;

public class RejectProposalEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/proposals/{id:guid}/reject")]
    public async Task<IActionResult> RejectProposal(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new RejectProposalCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors)
        );
    }
}