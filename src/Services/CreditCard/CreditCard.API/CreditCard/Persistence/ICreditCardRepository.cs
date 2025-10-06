namespace CreditCard.API.CreditCard.Persistence;

public interface ICreditCardRepository
{
    Task RequestCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken);
    Task<Models.CreditCard?> GetCreditCardByIdAsync(Guid creditCardId, CancellationToken cancellationToken);
    Task UpdateCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken);
}