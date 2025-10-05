using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.UpdateProposal;

public class UpdateProposalEndpoint(ISender sender) : ApiController
{
    [HttpPut("/api/proposals/{proposalId:guid}")]
    public async Task<IActionResult> UpdateProposal(
        [FromRoute] Guid proposalId,
        [FromBody] UpdateProposalRequest request,
        CancellationToken cancellationToken)
    {

        var command = new UpdateProposalCommand(proposalId, request.Amount);
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors)
        );
    }
}