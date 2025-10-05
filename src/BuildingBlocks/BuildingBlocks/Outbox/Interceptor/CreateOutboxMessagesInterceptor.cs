using System.Text.Json;
using BuildingBlocks.Domain;
using BuildingBlocks.Outbox.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Outbox.Interceptor;

public sealed class CreateOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEvents = context
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity.DomainEvents).ToList();

        var outboxMessagesFromDomainEvents = domainEvents
            .SelectMany(x => x)
            .Select(domainEvent => new OutboxMessage
            {
                Type = domainEvent.GetType().FullName!,
                OccurredOn = domainEvent.OccurredOn,
                Message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessagesFromDomainEvents);

        foreach (var entry in context.ChangeTracker.Entries<AggregateRoot>()) entry.Entity.ClearDomainEvents();

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}