using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using CreditCard.API.CreditCard.Models;
using CreditCard.API.CreditCard.Persistence;
using FluentValidation;

namespace CreditCard.API.CreditCard.RequestCreditCard;

public record RequestCreditCardCommand(
    Guid ClientId, 
    Guid ProposalId, 
    Money ExpensesLimit,
    CardProvider CardProvider)
    : ICommand<ErrorOr<Models.CreditCard>>;

public record RequestCreditCardResponse(Guid CreditCardId);

public class RequestCreditCardCommandHandler(ICreditCardRepository repository)
:ICommandHandler<RequestCreditCardCommand, ErrorOr<Models.CreditCard>>
{
    public async Task<ErrorOr<Models.CreditCard>> Handle(RequestCreditCardCommand command, CancellationToken cancellationToken)
    {
        var creditCard = Models.CreditCard.Create(command.ClientId, command.ProposalId, command.ExpensesLimit, command.CardProvider);
        
        await repository.RequestCreditCardAsync(creditCard, cancellationToken);

        return creditCard;
    }
}

public class RequestCreditCardCommandValidator : AbstractValidator<RequestCreditCardCommand>
{
    public RequestCreditCardCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.");
        RuleFor(x => x.ProposalId)
            .NotEmpty().WithMessage("ProposalId is required.");
        RuleFor(x => x.ExpensesLimit.Amount)
            .GreaterThan(0).WithMessage("ExpensesLimit must be greater than zero.");
        RuleFor(x => x.CardProvider)
            .IsInEnum().WithMessage("Invalid CardProvider.");
    }
}
