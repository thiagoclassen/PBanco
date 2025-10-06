using MongoDB.Driver;
using ReadService.API.Features.CreditCards.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.CreditCards.Repository;

public class CreditCardRepository(MongoDbContext context) : ICreditCardRepository
{
    private readonly IMongoCollection<CreditCardDocument> _collection = context.GetCollection<CreditCardDocument>("CreditCards");

    public async Task<CreditCardDocument?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CreditCardDocument>> GetByClientIdAsync(Guid clientId)
    {
        return await _collection.Find(c => c.ClientId == clientId).ToListAsync();
    }

    public async Task InsertOrUpdateAsync(CreditCardDocument card)
    {
        await _collection.ReplaceOneAsync(
            c => c.Id == card.Id,
            card,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}