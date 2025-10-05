using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.ListCreditCards;

public record ListCreditCardsQuery(Guid? ClientId, Guid? ProposalId) : IQuery<ErrorOr<List<Models.CreditCard>>>;

public class ListCreditCardsQueryHandler(ICreditCardRepository repository)
    : IQueryHandler<ListCreditCardsQuery, ErrorOr<List<Models.CreditCard>>>
{
    public async Task<ErrorOr<List<Models.CreditCard>>> Handle(ListCreditCardsQuery query,
        CancellationToken cancellationToken)
    {
        return (query.ClientId, query.ProposalId) switch
        {
            (null, null) => await repository.ListCreditCardsAsync(cancellationToken),
            (Guid clientId, Guid proposalId) => await repository.ListCreditCardsFromUserAndProposalAsync(clientId,
                proposalId, cancellationToken),
            (Guid clientId, null) => await repository.ListCreditCardsFromUserAsync(clientId, cancellationToken),
            (null, Guid proposalId) => await repository.ListCreditCardsFromProposalAsync(proposalId, cancellationToken)
        };
    }
}