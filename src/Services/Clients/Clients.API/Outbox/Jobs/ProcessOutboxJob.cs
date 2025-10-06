using BuildingBlocks.Domain.Events;
using BuildingBlocks.Outbox.Jobs;
using BuildingBlocks.UnitOfWork;
using MassTransit;

namespace Clients.API.Outbox.Jobs;

internal sealed class ProcessOutboxJob(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint,
    ILogger<ProcessOutboxJob> logger
) : IProcessOutboxJob
{
    public async Task<Task> ProcessAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("===> Starting ProcessAsync/Transaction");

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var messages = await unitOfWork.Outbox.GetUnprocessedMessagesAsync(cancellationToken);

        messages.ForEach(message => logger.LogInformation("===> Processing message {MessageId}", message.Id));

        await Task.WhenAll(messages.Select(async message =>
        {
            // Convert to concrete type otherwise the consumers will not be able to handle it
            var domainEvent = EventMapper.GetConcreteType(message.Type, message.Message);
            if (domainEvent is null) return;

            await publishEndpoint.Publish(domainEvent, cancellationToken);

            message.ProcessedOn = DateTime.UtcNow;
        }));

        unitOfWork.Outbox.MarkAsProcessed(messages);

        await unitOfWork.CommitAsync();

        logger.LogInformation("===> Finishing ProcessAsync/Transaction");
        return Task.CompletedTask;
    }
}