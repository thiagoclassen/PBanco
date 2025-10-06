using Clients.API.Client.Models;
using Clients.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Clients.API.Client.Persistence;

public class ClientRepository(ClientDbContext dbContext) : IClientRepository
{
    public async Task AddAsync(BankClient client, CancellationToken cancellationToken)
    {
        await dbContext.Clients.AddAsync(client, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        return await dbContext.Clients.AnyAsync(c => c.CPF == cpf, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Clients.AnyAsync(c => c.Email == email, cancellationToken);
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