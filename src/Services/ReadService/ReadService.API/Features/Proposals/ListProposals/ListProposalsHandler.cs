using BuildingBlocks.CQRS;
using ReadService.API.Features.Proposals.GetProposalById;
using ReadService.API.Features.Proposals.Models;
using ReadService.API.Features.Proposals.Repository;

namespace ReadService.API.Features.Proposals.ListProposals;

public record ListProposalsQuery(Guid? ClientId) : IQuery<ErrorOr<IEnumerable<ProposalResponse>>>;

public class ListProposalsQueryHandler(IProposalRepository repository)
    : IQueryHandler<ListProposalsQuery, ErrorOr<IEnumerable<ProposalResponse>>>
{
    public async Task<ErrorOr<IEnumerable<ProposalResponse>>> Handle(ListProposalsQuery query,
        CancellationToken cancellationToken)
    {
        IEnumerable<ProposalDocument> results;

        if (query.ClientId is not null)
            results = await repository.ListByClientIdAsync(query.ClientId.Value, cancellationToken);
        else
            results = await repository.ListAsync(cancellationToken);

        return results.Select(r => new ProposalResponse(r.Id,
                r.ClientId,
                r.ProposalStatus,
                r.ApprovedAmount.ToString()!))
            .ToList();
    }
}