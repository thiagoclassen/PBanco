using Clients.API.Models;

namespace Clients.API.Client.Interfaces;

public interface IClientRepository
{
    Task AddAsync(BankClient client, CancellationToken cancellationToken);
    Task <BankClient?> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);
    Task UpdateAsync(BankClient client, CancellationToken cancellationToken);
    Task DeleteAsync(BankClient client, CancellationToken cancellationToken);
}