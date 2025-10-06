using BuildingBlocks.CQRS;
using ReadService.API.Features.CreditCards.Repository;

namespace ReadService.API.Features.CreditCards.GetCreditCardById;

public record GetCreditCardByIdQuery(Guid CreditCardId) : IQuery<ErrorOr<CreditCardResponse?>>;

public record CreditCardResponse(
    Guid Id,
    Guid ClientId,
    int? Number,
    string ExpensesLimit,
    string CardProvider,
    string CardStatus);

public class GetCreditCardByIdQueryHandler(ICreditCardRepository repository)
    : IQueryHandler<GetCreditCardByIdQuery, ErrorOr<CreditCardResponse?>>
{
    public async Task<ErrorOr<CreditCardResponse?>> Handle(GetCreditCardByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(query.CreditCardId, cancellationToken);

        if (result is null) return Error.NotFound("CreditCard.NotFound", "Credit card not found");

        return new CreditCardResponse(result!.Id, result.ClientId, result.Number,
            result.ExpensesLimit.ToString()!, result.CardProvider, result.CardStatus);
    }
}