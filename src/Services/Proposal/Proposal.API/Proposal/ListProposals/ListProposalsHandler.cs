using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.ListProposals;

public record ListProposalsQuery(Guid? UserId) : IQuery<ErrorOr<IEnumerable<Models.Proposal>>>;

public class ListProposalsQueryHandler(IProposalRepository repository)
    : IQueryHandler<ListProposalsQuery, ErrorOr<IEnumerable<Models.Proposal>>>
{
    public async Task<ErrorOr<IEnumerable<Models.Proposal>>> Handle(ListProposalsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.UserId is not null)
            return await repository.GetByUserIdAsync(query.UserId.Value, cancellationToken);

        return await repository.GetAllAsync(cancellationToken);
    }
}