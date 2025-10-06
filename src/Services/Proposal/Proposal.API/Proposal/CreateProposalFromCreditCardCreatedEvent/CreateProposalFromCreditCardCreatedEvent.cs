using BuildingBlocks.CQRS;
using BuildingBlocks.UnitOfWork;
using MediatR;

namespace ProposalApi.Proposal.CreateProposalFromCreditCardCreatedEvent;

public record CreateProposalFromCreditCardCreatedEventCommand(Guid CreditCardId) : ICommand<Unit>;

public class CreateProposalFromCreditCardCreatedEventCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProposalFromCreditCardCreatedEventCommand, Unit>
{
    public async Task<Unit> Handle(CreateProposalFromCreditCardCreatedEventCommand command,
        CancellationToken cancellationToken)
    {
        var newProposal = Models.Proposal.Create(command.CreditCardId);
        await unitOfWork.Context.AddAsync(newProposal, cancellationToken);
        return Unit.Value;
    }
}