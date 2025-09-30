namespace ProposalApi.Proposal.Persistence;

public interface IProposalRepository
{
    Task AddAsync(Models.Proposal proposal, CancellationToken cancellationToken);
    Task<Models.Proposal?> GetByIdAsync(Guid proposalId, CancellationToken cancellationToken);
    Task UpdateAsync(Models.Proposal proposal, CancellationToken cancellationToken);
    Task<List<Models.Proposal>> GetAllAsync(CancellationToken cancellationToken);
    
}