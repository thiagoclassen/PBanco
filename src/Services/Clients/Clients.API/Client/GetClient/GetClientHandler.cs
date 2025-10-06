using BuildingBlocks.CQRS;
using Clients.API.Client.Models;
using Clients.API.Client.Persistence;

namespace Clients.API.Client.GetClient;

public record GetClientQuery(Guid ClientId) : IQuery<ErrorOr<BankClient?>>;

public record GetClientResult(
    Guid Id,
    string Name,
    string Email,
    string CPF,
    DateTime BirthDate,
    DateTime CreatedAt,
    DateTime UpdateAt);

public class GetClientHandler(IClientRepository repository) : IQueryHandler<GetClientQuery, ErrorOr<BankClient?>>
{
    public async Task<ErrorOr<BankClient?>> Handle(GetClientQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(query.ClientId, cancellationToken);
    }
}