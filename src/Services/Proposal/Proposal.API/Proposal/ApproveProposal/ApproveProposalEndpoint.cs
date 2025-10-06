using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.ApproveProposal;

public class ApproveProposalEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/proposals/{id:guid}/approve")]
    public async Task<IActionResult> ApproveProposal(
        [FromRoute] Guid id,
        [FromBody] ApproveProposalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ApproveProposalCommand(id, request.ApprovedAmount, request.ApprovedCurrency!);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors)
        );
    }
}