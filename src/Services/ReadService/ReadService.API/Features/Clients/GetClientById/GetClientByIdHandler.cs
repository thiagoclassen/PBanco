using BuildingBlocks.CQRS;
using ReadService.API.Features.Clients.Repository;

namespace ReadService.API.Features.Clients.GetClientById;

public record GetClientByIdQuery(Guid ClientId) : IQuery<ErrorOr<GetClientResult?>>;

public record GetClientResult(
    Guid Id,
    string Name,
    string Email,
    string CPF,
    DateTime BirthDate);

public class GetClientByIdQueryHandler(IClientRepository repository)
    : IQueryHandler<GetClientByIdQuery, ErrorOr<GetClientResult?>>
{
    public async Task<ErrorOr<GetClientResult?>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(query.ClientId, cancellationToken);
        if (result is null) return Error.NotFound("Client.NotFound", "Client not found");

        return new GetClientResult(
            result.Id,
            result.Name,
            result.Email,
            result.CPF,
            result.BirthDate);
    }
}