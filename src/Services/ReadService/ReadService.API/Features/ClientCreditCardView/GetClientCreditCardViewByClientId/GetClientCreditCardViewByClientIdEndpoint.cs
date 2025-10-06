using BuildingBlocks.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReadService.API.Features.ClientCreditCardView.GetClientCreditCardViewByClientId;

public class GetClientCreditCardViewByClientIdEndpoint(ISender sender) : ApiController
{
    [HttpGet("api/client-credit-card-view/{clientId:guid}")]
    public async Task<IActionResult> GetClientCreditCardViewByClientId([FromRoute] Guid clientId,
        CancellationToken cancellationToken)
    {
        var query = new GetCreditCardViewByClientIdQuery(clientId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response),
            errors => Problem(errors));
    }
}