using CreditCard.API.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API.CreditCard.Persistence;

public class CreditCardRepository(CreditCardDbContext dbContext) : ICreditCardRepository
{
    public async Task RequestCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken)
    {
        await dbContext.CreditCards.AddAsync(creditCard, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Models.CreditCard?> GetCreditCardByIdAsync(Guid creditCardId, CancellationToken cancellationToken)
    {
        return await dbContext.CreditCards.FindAsync([creditCardId], cancellationToken);
    }

    public Task UpdateCreditCardAsync(Models.CreditCard creditCard, CancellationToken cancellationToken)
    {
        dbContext.CreditCards.Update(creditCard);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Models.CreditCard>> ListCreditCardsFromUserAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.CreditCards.Where(card => card.ClientId == userId).ToListAsync(cancellationToken);
    }

    public async Task<List<Models.CreditCard>> ListCreditCardsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.CreditCards.ToListAsync(cancellationToken);
    }

    public async Task<List<Models.CreditCard>> ListCreditCardsFromProposalAsync(Guid proposalId,
        CancellationToken cancellationToken)
    {
        return await dbContext.CreditCards.Where(card => card.ProposalId == proposalId).ToListAsync(cancellationToken);
    }

    public async Task<List<Models.CreditCard>> ListCreditCardsFromUserAndProposalAsync(Guid clientId, Guid proposalId,
        CancellationToken cancellationToken)
    {
        return await dbContext.CreditCards.Where(card => card.ProposalId == proposalId && card.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }
}