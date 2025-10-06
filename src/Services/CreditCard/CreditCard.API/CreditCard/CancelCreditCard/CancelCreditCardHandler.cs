using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Exceptions;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.CancelCreditCard;

public record CancelCreditCardCommand(Guid CreditCardId) : ICommand<ErrorOr<CancelCreditCardResult>>;

public record CancelCreditCardResult;

public class CancelCreditCardCommandHandler(ICreditCardRepository repository)
    : ICommandHandler<CancelCreditCardCommand, ErrorOr<CancelCreditCardResult>>
{
    public async Task<ErrorOr<CancelCreditCardResult>> Handle(CancelCreditCardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var creditCard = await repository.GetCreditCardByIdAsync(request.CreditCardId, cancellationToken);
            if (creditCard is null) return Error.NotFound("CreditCard.NotFound", "Credit card not found");

            creditCard.Cancel();

            await repository.UpdateCreditCardAsync(creditCard, cancellationToken);
        }
        catch (InvalidCardStatusStateException e)
        {
            return Error.Forbidden("CreditCard.InvalidState", e.Message);
        }

        return new CancelCreditCardResult();
    }
}