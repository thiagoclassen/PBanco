using BuildingBlocks.Domain.Shared;
using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProposalApi.Proposal.CreateProposal;

public class CreateProposalEndpoint(ISender sender) : ApiController
{
    [HttpPost("/api/proposals")]
    public async Task<IActionResult> CreateProposal(
        [FromBody] CreateProposalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProposalCommand(request.ClientId);
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(
            _ => Created($"/api/proposals/{result.Value.Id}", result.Value.MapToProposalResponse()),
            errors => Problem(errors)
        );
    }
}