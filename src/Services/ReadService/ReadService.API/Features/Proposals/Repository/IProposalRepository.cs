using ReadService.API.Features.Proposals.Models;

namespace ReadService.API.Features.Proposals.Repository;

public interface IProposalRepository
{
    Task<ProposalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<IEnumerable<ProposalDocument>> ListByClientIdAsync(Guid clientId, CancellationToken cancellationToken);
    public Task<IEnumerable<ProposalDocument>> ListAsync(CancellationToken cancellationToken);
    Task InsertOrUpdateAsync(ProposalDocument proposal);
}