using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.GetCreditCardById;

public record GetCreditCardByIdQuery(Guid CreditCardId) : IQuery<ErrorOr<Models.CreditCard?>>;

public class GetCreditCardByIdQueryHandler(ICreditCardRepository repository)
    : IQueryHandler<GetCreditCardByIdQuery, ErrorOr<Models.CreditCard?>>
{
    public async Task<ErrorOr<Models.CreditCard?>> Handle(GetCreditCardByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await repository.GetCreditCardByIdAsync(query.CreditCardId, cancellationToken);
    }
}