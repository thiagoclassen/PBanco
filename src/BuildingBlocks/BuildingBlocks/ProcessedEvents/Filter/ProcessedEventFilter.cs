using System.Text.Json;
using BuildingBlocks.Domain;
using BuildingBlocks.ProcessedEvents.Models;
using BuildingBlocks.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.ProcessedEvents.Filter;

public class ProcessedEventFilter<T>(
    IUnitOfWork unitOfWork,
    ILogger<ProcessedEventFilter<T>> logger)
    : IFilter<ConsumeContext<T>>
    where T : class, IDomainEvent
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var eventId = context.Message.EventId;

        if (await unitOfWork.ProcessedEvents.ExistsAsync(eventId, context.CancellationToken))
        {
            logger.LogInformation("Event {EventName} with ID {EventId} has already been processed. Skipping.",
                context.Message.EventName, context.Message.EventId);
            return;
        }

        await unitOfWork.BeginTransactionAsync(context.CancellationToken);

        var eventProcessedEntry = new ProcessedEvent
        {
            EventId = context.Message.EventId,
            EventName = context.Message.EventName,
            Message = JsonSerializer.Serialize(context.Message),
            ProcessedAt = DateTime.UtcNow
        };

        await unitOfWork.ProcessedEvents.AddAsync(eventProcessedEntry, context.CancellationToken);

        await next.Send(context);

        await unitOfWork.CommitAsync();
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("ProcessedEventFilter");
    }
}