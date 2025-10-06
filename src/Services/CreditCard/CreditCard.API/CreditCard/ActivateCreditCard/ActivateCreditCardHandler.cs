using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Exceptions;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.ActivateCreditCard;

public record ActivateCreditCardCommand(Guid CreditCardId) : ICommand<ErrorOr<ActivateCreditCardResult>>;

public record ActivateCreditCardResult;

public class ActivateCreditCardCommandHandler(ICreditCardRepository repository)
    : ICommandHandler<ActivateCreditCardCommand, ErrorOr<ActivateCreditCardResult>>
{
    public async Task<ErrorOr<ActivateCreditCardResult>> Handle(ActivateCreditCardCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var creditCard = await repository.GetCreditCardByIdAsync(command.CreditCardId, cancellationToken);

            if (creditCard is null) return Error.NotFound("CreditCard.NotFound", "Credit card not found");

            creditCard.Activate();

            await repository.UpdateCreditCardAsync(creditCard, cancellationToken);
        }
        catch (InvalidCardStatusStateException e)
        {
            return Error.Forbidden("CreditCard.InvalidState", e.Message);
        }


        return new ActivateCreditCardResult();
    }
}