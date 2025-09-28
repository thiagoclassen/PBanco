using System.Text.Json;
using Clients.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Clients.API.Data.Interceptors;

public sealed class CreateOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = context
            .ChangeTracker
            .Entries<BankClient>()
            .Where(x => x.State == EntityState.Added)
            .Select(x =>
                new OutboxMessage()
                {
                    Type = "BankClientCreated",
                    OccurredOn = DateTime.UtcNow,
                    Message = JsonSerializer.Serialize(new
                    {
                        x.Entity.Id,
                        x.Entity.Name,
                        x.Entity.CPF,
                        x.Entity.Email,
                        x.Entity.CreatedAt
                    })
                }
            )
            .ToList();
        
        context.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}