using ReadService.API.Features.Clients.Models;

namespace ReadService.API.Features.Clients.Repository;

public interface IClientRepository
{
    Task<ClientDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task InsertOrUpdateAsync(ClientDocument client);
    Task<IEnumerable<ClientDocument>> GetAllAsync();
}