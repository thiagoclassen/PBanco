using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCard.API.CreditCard.RequestCreditCard;

public class RequestCreditCardEndpoint(ISender sender) : ApiController
{
    [HttpPost("/api/credit-cards")]
    public async Task<IActionResult> RequestCreditCard(
        [FromBody] CreditCardRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.MapToCreditCardCommand();
        var result = await sender.Send(command, cancellationToken);
        return result.Match(
            creditCard => Created($"/api/credit-cards/{creditCard.Id}", creditCard.MapToCreditCardResponse()),
            errors => Problem(errors)
        );
    }
}