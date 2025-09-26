using BuildingBlocks.CQRS;
using Clients.API.Client.Interfaces;
using Clients.API.Models;


namespace Clients.API.Client.GetClient;

public record GetClientQuery(Guid ClientId) : ICommand<ErrorOr<BankClient?>>;

public record GetClientResult(
    Guid Id,
    string Name,
    string Email,
    string CPF,
    DateTime BirthDate,
    DateTime CreatedAt,
    DateTime UpdateAt);

public class GetClientHandler(IClientRepository repository) : ICommandHandler<GetClientQuery, ErrorOr<BankClient?>>
{
    public async Task<ErrorOr<BankClient?>> Handle(GetClientQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(query.ClientId, cancellationToken);
    }
}