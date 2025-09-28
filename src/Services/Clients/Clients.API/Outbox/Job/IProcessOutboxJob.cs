namespace Clients.API.Outbox.Job;

public interface IProcessOutboxJob
{
    Task<Task> ProcessAsync(CancellationToken cancellationToken = default);
}