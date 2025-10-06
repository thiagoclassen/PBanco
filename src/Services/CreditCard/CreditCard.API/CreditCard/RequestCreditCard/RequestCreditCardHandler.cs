using BuildingBlocks.CQRS;
using BuildingBlocks.Domain.Shared;
using CreditCard.API.CreditCard.Models;
using CreditCard.API.CreditCard.Persistence;
using FluentValidation;

namespace CreditCard.API.CreditCard.RequestCreditCard;

public record RequestCreditCardCommand(
    Guid ClientId,
    string CardProvider)
    : ICommand<ErrorOr<RequestCreditCardResponse>>;

public record RequestCreditCardResponse(
    Guid Id,
    Guid ClientId,
    string Status,
    string CardProvider,
    int? Number,
    string ExpensesLimit,
    DateTime UpdatedAt);

public class RequestCreditCardCommandHandler(ICreditCardRepository repository)
    : ICommandHandler<RequestCreditCardCommand, ErrorOr<RequestCreditCardResponse>>
{
    public async Task<ErrorOr<RequestCreditCardResponse>> Handle(RequestCreditCardCommand command,
        CancellationToken cancellationToken)
    {
        var creditCard = Models.CreditCard.Create(
            command.ClientId,
            Enum.Parse<CardProvider>(command.CardProvider));

        await repository.RequestCreditCardAsync(creditCard, cancellationToken);

        return new RequestCreditCardResponse(creditCard.Id, creditCard.ClientId, creditCard.Status.ToString(),
            creditCard.CardProvider.ToString(), creditCard.Number, creditCard.ExpensesLimit.ToString(),
            creditCard.UpdatedAt);;
    }
}

public class RequestCreditCardCommandValidator : AbstractValidator<RequestCreditCardCommand>
{
    public RequestCreditCardCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.");
        RuleFor(x => x.CardProvider)
            .Must(cardProvider => Enum.TryParse(typeof(CardProvider), cardProvider, true, out _))
            .WithMessage($"Provider must be: {string.Join(", ", Enum.GetNames(typeof(CardProvider)))}");
    }
}