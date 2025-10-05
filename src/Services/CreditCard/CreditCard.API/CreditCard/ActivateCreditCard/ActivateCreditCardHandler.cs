using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.ActivateCreditCard;

public record ActivateCreditCardCommand(Guid CreditCardId) : ICommand<ErrorOr<ActivateCreditCardResult>>;

public record ActivateCreditCardResult(Models.CreditCard CreditCard);

public class ActivateCreditCardCommandHandler(ICreditCardRepository repository)
    : ICommandHandler<ActivateCreditCardCommand, ErrorOr<ActivateCreditCardResult>>
{
    public async Task<ErrorOr<ActivateCreditCardResult>> Handle(ActivateCreditCardCommand command,
        CancellationToken cancellationToken)
    {
        var creditCard = await repository.GetCreditCardByIdAsync(command.CreditCardId, cancellationToken);

        if (creditCard is null) return Error.NotFound("CreditCard.NotFound", "Credit card not found");

        creditCard.Activate();

        await repository.UpdateCreditCardAsync(creditCard, cancellationToken);

        return new ActivateCreditCardResult(creditCard);
    }
}