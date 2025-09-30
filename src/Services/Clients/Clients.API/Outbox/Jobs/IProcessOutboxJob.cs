namespace Clients.API.Outbox.Jobs;

public interface IProcessOutboxJob
{
    Task<Task> ProcessAsync(CancellationToken cancellationToken = default);
}