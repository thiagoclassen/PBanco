using BuildingBlocks.Http;
using MediatR;

namespace CreditCard.API.CreditCard.ListCreditCards;

public class ListCreditCardsEndpoint(ISender sender) : ApiController
{
    //[HttpGet("/api/credit-cards")]
    // public async Task<IActionResult> ListCreditCards(
    //     [FromQuery] Guid? clientId,
    //     [FromQuery] Guid? proposalId,
    //     CancellationToken cancellationToken)
    // {
    //     var query = new ListCreditCardsQuery(clientId, proposalId);
    //     var result = await sender.Send(query, cancellationToken);
    //     return result.Match(
    //         creditCards => Ok(creditCards.Select(cc => cc.MapToCreditCardResponse())),
    //         errors => Problem(errors));
    // }
}