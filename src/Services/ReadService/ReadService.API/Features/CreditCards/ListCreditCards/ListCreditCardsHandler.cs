using BuildingBlocks.CQRS;
using ReadService.API.Features.CreditCards.GetCreditCardById;
using ReadService.API.Features.CreditCards.Repository;

namespace ReadService.API.Features.CreditCards.ListCreditCards;

public record ListCreditCardsQuery(Guid? ClientId, Guid? ProposalId)
    : IQuery<ErrorOr<IEnumerable<CreditCardResponse>>>;

public class ListCreditCardsQueryHandler(ICreditCardRepository repository)
    : IQueryHandler<ListCreditCardsQuery, ErrorOr<IEnumerable<CreditCardResponse>>>
{
    public async Task<ErrorOr<IEnumerable<CreditCardResponse>>> Handle(ListCreditCardsQuery query,
        CancellationToken cancellationToken)
    {
        var response = (query.ClientId, query.ProposalId) switch
        {
            (null, null) => await repository.ListAsync(cancellationToken),
            (Guid clientId, Guid proposalId) => await repository.ListByClientIdAndProposalIdAsync(clientId,
                proposalId, cancellationToken),
            (Guid clientId, null) => await repository.ListByClientIdAsync(clientId, cancellationToken),
            (null, Guid proposalId) => await repository.ListByProposalIdAsync(proposalId, cancellationToken)
        };

        return response
            .Select(c => new CreditCardResponse(
                c!.Id,
                c.ClientId,
                c.ProposalId,
                c.Number,
                c.ExpensesLimit.ToString()!,
                c.CardProvider,
                c.CardStatus))
            .ToList();
    }
}