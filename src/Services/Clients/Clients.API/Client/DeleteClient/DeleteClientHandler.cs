using BuildingBlocks.CQRS;
using Clients.API.Client.Persistence;

namespace Clients.API.Client.DeleteClient;

public record DeleteClientCommand(Guid ClientId) : ICommand<ErrorOr<DeleteClientResult>>;

public record DeleteClientResult(bool Success);

public class DeleteClientCommandHandler(IClientRepository repository)
    : ICommandHandler<DeleteClientCommand, ErrorOr<DeleteClientResult>>
{
    public async Task<ErrorOr<DeleteClientResult>> Handle(DeleteClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(command.ClientId, cancellationToken);
        if (client is null)
            return new DeleteClientResult(false);

        client.Delete();

        await repository.DeleteAsync(client, cancellationToken);
        return new DeleteClientResult(true);
    }
}