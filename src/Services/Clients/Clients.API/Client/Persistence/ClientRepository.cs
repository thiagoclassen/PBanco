using Clients.API.Data;
using Clients.API.Models;

namespace Clients.API.Client.Persistence;

public class ClientRepository(ClientDbContext dbContext) : IClientRepository
{
    public async Task AddAsync(BankClient client, CancellationToken cancellationToken)
    {
        await dbContext.Clients.AddAsync(client, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<BankClient?> GetByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return await dbContext.Clients.FindAsync([clientId], cancellationToken);
    }

    public async Task UpdateAsync(BankClient client, CancellationToken cancellationToken)
    {
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(BankClient client, CancellationToken cancellationToken)
    {
        dbContext.Clients.Remove(client);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}