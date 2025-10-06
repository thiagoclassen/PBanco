using ReadService.API.Features.Proposals.Models;

namespace ReadService.API.Features.Proposals.Repository;

public interface IProposalRepository
{
    Task<ProposalDocument?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProposalDocument>> GetByClientIdAsync(Guid clientId);
    Task InsertOrUpdateAsync(ProposalDocument proposal);
}