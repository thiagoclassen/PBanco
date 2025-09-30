using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.GetProposalById;

public record GetProposalByIdQuery(Guid ProposalId) : IQuery<ErrorOr<Models.Proposal?>>;

public class GetProposalByIdQueryHandler(IProposalRepository repository) : IQueryHandler<GetProposalByIdQuery, ErrorOr<Models.Proposal?>>
{
    public async Task<ErrorOr<Models.Proposal?>> Handle(GetProposalByIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(query.ProposalId, cancellationToken);
    }
}
