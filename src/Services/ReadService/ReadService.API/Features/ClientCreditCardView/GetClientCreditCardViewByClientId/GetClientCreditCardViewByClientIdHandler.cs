using BuildingBlocks.CQRS;
using ReadService.API.Features.ClientCreditCardView.Models;
using ReadService.API.Features.ClientCreditCardView.Repository;

namespace ReadService.API.Features.ClientCreditCardView.GetClientCreditCardViewByClientId;

public record GetCreditCardViewByClientIdQuery(Guid ClientId) : IQuery<ErrorOr<GetCreditCardViewByClientIdResponse>>;

public record GetCreditCardViewByClientIdResponse(
    Guid ResultClientId,
    string ResultName,
    DateTime ResultBirthDate,
    List<CardSummary> ResultCreditCards);

public class GetCreditCardViewByClientIdQueryHandler(IClientCreditCardViewRepository repository)
    : IQueryHandler<GetCreditCardViewByClientIdQuery, ErrorOr<GetCreditCardViewByClientIdResponse>>
{
    public async Task<ErrorOr<GetCreditCardViewByClientIdResponse>> Handle(GetCreditCardViewByClientIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.FindCreditCardViewByClientIdAsync(query.ClientId);

        if (result is null) return Error.NotFound("CreditCardView.NotFound", "Credit card view not found");

        return new GetCreditCardViewByClientIdResponse(
            result.ClientId,
            result.Name,
            result.BirthDate,
            result.CreditCards);
    }
}