using BuildingBlocks.CQRS;
using ProposalApi.Proposal.Model;
using ProposalApi.Proposal.Models;
using ProposalApi.Proposal.Persistence;

namespace ProposalApi.Proposal.CreateProposal;

public record CreateProposalCommand(Guid ClientId) : ICommand<ErrorOr<Models.Proposal>>;

public record CreateProposalResult(Guid Id);

public class CreateProposalCommandHandler(IProposalRepository repository)
    : ICommandHandler<CreateProposalCommand, ErrorOr<Models.Proposal>>
{
    public async Task<ErrorOr<Models.Proposal>> Handle(CreateProposalCommand command, CancellationToken cancellationToken)
    {
        var proposal = new Models.Proposal
        {
            Id = Guid.NewGuid(),
            ClientId = command.ClientId,
            Requested = DateTime.UtcNow,
            ProposalStatus = ProposalStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        await repository.AddAsync(proposal, cancellationToken);

        return proposal;
    }
}

