using System.Text.Json;
using Clients.API.Client.Models;
using Clients.API.Data;
using Clients.API.Messages;

namespace Clients.API.Outbox.Job;

internal sealed class ProcessOutboxJob(
    UnitOfWork unitOfWork,
    MessageService messageService,
    ILogger<ProcessOutboxJob> logger
) : IProcessOutboxJob
{
    public async Task<Task> ProcessAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("===> Starting ProcessAsync/Transaction");

        await unitOfWork.BeginTransactionAsync();

        var messages = await unitOfWork.Outbox.GetUnprocessedMessagesAsync(cancellationToken: cancellationToken);

        // var messages = await  repository.GetUnprocessedMessagesAsync(cancellationToken: cancellationToken);
        messages.ForEach(message => logger.LogInformation("===> Processing message {MessageId}", message.Id));

        await Task.WhenAll(messages.Select(async message =>
        {
            var client = JsonSerializer.Deserialize<BankClient>(message.Message);
            await messageService.PublishClientCreated(client!.Id);
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