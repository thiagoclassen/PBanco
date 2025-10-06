using MongoDB.Driver;
using ReadService.API.Features.Clients.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.Clients.Repository;

public class ClientRepository(MongoDbContext context) : IClientRepository
{
    private readonly IMongoCollection<ClientDocument> _collection = context.GetCollection<ClientDocument>("Clients");

    public async Task<ClientDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClientDocument>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task InsertOrUpdateAsync(ClientDocument client)
    {
        await _collection.ReplaceOneAsync(
            c => c.Id == client.Id,
            client,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}