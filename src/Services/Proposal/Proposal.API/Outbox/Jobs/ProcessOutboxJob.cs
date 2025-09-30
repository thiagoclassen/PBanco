using BuildingBlocks.Events;
using BuildingBlocks.Outbox;
using BuildingBlocks.Outbox.Jobs;
using MassTransit;
using ProposalApi.Data;

namespace ProposalApi.Outbox.Jobs;

internal sealed class ProcessOutboxJob(
    UnitOfWork<ProposalDbContext> unitOfWork,
    IPublishEndpoint publishEndpoint,
    ILogger<ProcessOutboxJob> logger
) : IProcessOutboxJob
{
    public async Task<Task> ProcessAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("===> Starting ProcessAsync/Transaction");

        await unitOfWork.BeginTransactionAsync();

        var messages = await unitOfWork.Outbox.GetUnprocessedMessagesAsync(cancellationToken: cancellationToken);

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

    public Task ProcessAsync()
    {
        throw new NotImplementedException();
    }
}