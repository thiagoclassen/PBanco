using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCard.API.CreditCard.GetCreditCardByIdAsync;

public class GetCreditCardByIdEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/credit-cards/{creditCardId:guid}")]
    public async Task<IActionResult> GetCreditCardById(
        [FromRoute] Guid creditCardId,
        CancellationToken cancellationToken)
    {
        var query = new GetCreditCardByIdQuery(creditCardId);
        var result = await sender.Send(query, cancellationToken);
        return result.Match(
            _ => result.Value is null ? NoContent() : Ok(result.Value.MapToCreditCardResponse()),
            errors => Problem(errors)
        );
    }
}