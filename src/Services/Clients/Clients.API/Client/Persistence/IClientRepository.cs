using Clients.API.Client.Models;

namespace Clients.API.Client.Persistence;

public interface IClientRepository
{
    Task AddAsync(BankClient client, CancellationToken cancellationToken);
    Task<BankClient?> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);
    Task UpdateAsync(BankClient client, CancellationToken cancellationToken);
    Task DeleteAsync(BankClient client, CancellationToken cancellationToken);
}