using ReadService.API.Features.CreditCards.Models;

namespace ReadService.API.Features.CreditCards.Repository;

public interface ICreditCardRepository
{
    Task<CreditCardDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<CreditCardDocument>> ListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<CreditCardDocument>> ListByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<IEnumerable<CreditCardDocument>> ListByProposalIdAsync(Guid proposalId,
        CancellationToken cancellationToken);

    Task<IEnumerable<CreditCardDocument>> ListByClientIdAndProposalIdAsync(Guid clientId, Guid proposalId,
        CancellationToken cancellationToken);

    Task InsertOrUpdateAsync(CreditCardDocument card, CancellationToken cancellationToken);
}