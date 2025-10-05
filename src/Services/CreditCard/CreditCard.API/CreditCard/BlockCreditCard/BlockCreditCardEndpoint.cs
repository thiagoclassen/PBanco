using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCard.API.CreditCard.BlockCreditCard;

public class BlockCreditCardEndpoint(ISender sender) : ApiController
{
    [HttpPost("api/credit-cards/{id:guid}/block")]
    public async Task<IActionResult> BlockCreditCard(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new BlockCreditCardCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}