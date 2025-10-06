using MongoDB.Driver;
using ReadService.API.Features.Proposals.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.Proposals.Repository;

public class ProposalRepository(MongoDbContext context) : IProposalRepository
{
    private readonly IMongoCollection<ProposalDocument> _collection =
        context.GetCollection<ProposalDocument>("Proposals");

    public async Task<ProposalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProposalDocument>> ListByClientIdAsync(Guid clientId,
        CancellationToken cancellationToken)
    {
        return await _collection.Find(p => p.ClientId == clientId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProposalDocument>> ListAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
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