using MongoDB.Driver;
using ReadService.API.Features.Proposals.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.Proposals.Repository;

public class ProposalRepository(MongoDbContext context) : IProposalRepository
{
    private readonly IMongoCollection<ProposalDocument> _collection = context.GetCollection<ProposalDocument>("Proposals");

    public async Task<ProposalDocument?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProposalDocument>> GetByClientIdAsync(Guid clientId)
    {
        return await _collection.Find(p => p.ClientId == clientId).ToListAsync();
    }

    public async Task InsertOrUpdateAsync(ProposalDocument proposal)
    {
        await _collection.ReplaceOneAsync(
            p => p.Id == proposal.Id,
            proposal,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}