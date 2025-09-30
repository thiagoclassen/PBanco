using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.ListProposals;

public record ListProposalsQuery() : IQuery<ErrorOr<IEnumerable<Models.Proposal>>>;

public class ListProposalsQueryHandler(IProposalRepository repository) : IQueryHandler<ListProposalsQuery, ErrorOr<IEnumerable<Models.Proposal>>>
{
    public async Task<ErrorOr<IEnumerable<Models.Proposal>>> Handle(ListProposalsQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}