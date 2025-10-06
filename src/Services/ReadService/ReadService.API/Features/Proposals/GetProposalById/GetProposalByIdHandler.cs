using BuildingBlocks.CQRS;
using ReadService.API.Features.Proposals.Repository;

namespace ReadService.API.Features.Proposals.GetProposalById;

public record GetProposalByIdQuery(Guid ProposalId) : IQuery<ErrorOr<ProposalResponse?>>;

public record ProposalResponse(Guid Id, Guid CreditCardId, string Status, string ApprovedAmmount);

public class GetProposalByIdQueryHandler(IProposalRepository repository)
    : IQueryHandler<GetProposalByIdQuery, ErrorOr<ProposalResponse?>>
{
    public async Task<ErrorOr<ProposalResponse?>> Handle(GetProposalByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(query.ProposalId, cancellationToken);
        if (result is null) return Error.NotFound("Proposal.NotFound", "Proposal not found");

        return new ProposalResponse(
            result.Id,
            result.CreditCardId,
            result.ProposalStatus,
            result.ApprovedAmount.ToString()!);
    }
}