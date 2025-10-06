using BuildingBlocks.CQRS;
using ReadService.API.Features.CreditCards.GetCreditCardById;
using ReadService.API.Features.CreditCards.Repository;

namespace ReadService.API.Features.CreditCards.ListCreditCards;

public record ListCreditCardsQuery(Guid? ClientId)
    : IQuery<ErrorOr<IEnumerable<CreditCardResponse>>>;

public class ListCreditCardsQueryHandler(ICreditCardRepository repository)
    : IQueryHandler<ListCreditCardsQuery, ErrorOr<IEnumerable<CreditCardResponse>>>
{
    public async Task<ErrorOr<IEnumerable<CreditCardResponse>>> Handle(ListCreditCardsQuery query,
        CancellationToken cancellationToken)
    {
        var response = (query.ClientId) switch
        {
            (null) => await repository.ListAsync(cancellationToken),
            (Guid clientId) => await repository.ListByClientIdAsync(clientId, cancellationToken)
        };

        return response
            .Select(c => new CreditCardResponse(
                c!.Id,
                c.ClientId,
                c.Number,
                c.ExpensesLimit.ToString()!,
                c.CardProvider,
                c.CardStatus))
            .ToList();
    }
}