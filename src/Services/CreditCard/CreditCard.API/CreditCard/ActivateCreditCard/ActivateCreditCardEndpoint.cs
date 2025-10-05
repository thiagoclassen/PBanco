using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCard.API.CreditCard.ActivateCreditCard;

public class ActivateCreditCardEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/credit-cards/{creditCardId:guid}/activate")]
    public async Task<IActionResult> ApproveProposal(
        [FromRoute]Guid creditCardId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateCreditCardCommand(creditCardId);
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}