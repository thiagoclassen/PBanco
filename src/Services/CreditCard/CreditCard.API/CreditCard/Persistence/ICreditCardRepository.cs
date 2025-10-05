namespace CreditCard.API.CreditCard.Persistence;

public interface ICreditCardRepository
{
    Task RequestCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken);
    Task<Models.CreditCard?> GetCreditCardByIdAsync(Guid creditCardId, CancellationToken cancellationToken);
    Task UpdateCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken);
    Task<List<Models.CreditCard>> ListCreditCardsFromUserAsync(Guid clientId, CancellationToken cancellationToken);
    Task<List<Models.CreditCard>> ListCreditCardsAsync(CancellationToken cancellationToken);

    Task<List<Models.CreditCard>>
        ListCreditCardsFromProposalAsync(Guid proposalId, CancellationToken cancellationToken);

    Task<List<Models.CreditCard>> ListCreditCardsFromUserAndProposalAsync(Guid clientId, Guid proposalId,
        CancellationToken cancellationToken);
}