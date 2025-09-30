using Microsoft.EntityFrameworkCore;
using ProposalApi.Data;

namespace ProposalApi.Proposal.Persistence;

public class ProposalRepository
    (ProposalDbContext dbContext): IProposalRepository
{
    public async Task AddAsync(Models.Proposal proposal, CancellationToken cancellationToken)
    {
        await dbContext.Proposals.AddAsync(proposal, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Models.Proposal?> GetByIdAsync(Guid proposalId, CancellationToken cancellationToken)
    {
        return await dbContext.Proposals.FindAsync([proposalId], cancellationToken);
    }

    public async Task UpdateAsync(Models.Proposal proposal, CancellationToken cancellationToken)
    {
        dbContext.Proposals.Update(proposal);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Models.Proposal>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Proposals.ToListAsync(cancellationToken);
    }
}