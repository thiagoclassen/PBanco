using MongoDB.Driver;
using ReadService.API.Features.CreditCards.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.CreditCards.Repository;

public class CreditCardRepository(MongoDbContext context) : ICreditCardRepository
{
    private readonly IMongoCollection<CreditCardDocument> _collection =
        context.GetCollection<CreditCardDocument>("CreditCards");

    public async Task<CreditCardDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<CreditCardDocument>> ListAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CreditCardDocument>> ListByClientIdAsync(Guid clientId,
        CancellationToken cancellationToken)
    {
        return await _collection.Find(c => c.ClientId == clientId).ToListAsync(cancellationToken);
    }

    public async Task InsertOrUpdateAsync(CreditCardDocument card, CancellationToken cancellationToken)
    {
        await _collection.ReplaceOneAsync(
            c => c.Id == card.Id,
            card,
            new ReplaceOptions { IsUpsert = true }
            , cancellationToken);
    }
}