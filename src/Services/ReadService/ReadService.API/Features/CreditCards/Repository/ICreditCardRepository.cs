using ReadService.API.Features.CreditCards.Models;

namespace ReadService.API.Features.CreditCards.Repository;

public interface ICreditCardRepository
{
    Task<CreditCardDocument?> GetByIdAsync(Guid id);
    Task<IEnumerable<CreditCardDocument>> GetByClientIdAsync(Guid clientId);
    Task InsertOrUpdateAsync(CreditCardDocument card);
}