using ReadService.API.Features.CreditCards.Models;

namespace ReadService.API.Features.CreditCards.Repository;

public interface ICreditCardRepository
{
    Task<CreditCardDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<CreditCardDocument>> ListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<CreditCardDocument>> ListByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task InsertOrUpdateAsync(CreditCardDocument card, CancellationToken cancellationToken);
}