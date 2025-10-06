using BuildingBlocks.Http;
using CreditCard.API.CreditCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReadService.API.Features.CreditCards.ListCreditCards;

public class ListCreditCardsEndpoint(ISender sender) : ApiController
{
    [HttpGet("/api/credit-cards")]
    public async Task<IActionResult> ListCreditCards(
        [FromQuery] Guid? clientId,
        [FromQuery] Guid? proposalId,
        CancellationToken cancellationToken)
    {
        var query = new ListCreditCardsQuery(clientId, proposalId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.Match(
            _ => Ok(result.Value),
            errors => Problem(errors));
    }
}