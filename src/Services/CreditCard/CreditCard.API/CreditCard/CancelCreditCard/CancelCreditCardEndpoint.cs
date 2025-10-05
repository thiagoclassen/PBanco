using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCard.API.CreditCard.CancelCreditCard;

public class CancelCreditCardEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/credit-cards/{creditCardId:guid}/cancel")]
    public async Task<IActionResult> CancelCreditCard(
        [FromRoute] Guid creditCardId,
        CancellationToken cancellationToken)
    {
        var command = new CancelCreditCardCommand(creditCardId);
        var result = await sender.Send(command, cancellationToken);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}