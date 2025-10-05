using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.CancelCreditCard;

public record CancelCreditCardCommand(Guid CreditCardId) : ICommand<ErrorOr<CancelCreditCardResult>>;
public record CancelCreditCardResult(Models.CreditCard CreditCard);

public class CancelCreditCardCommandHandler(ICreditCardRepository repository) : ICommandHandler<CancelCreditCardCommand, ErrorOr<CancelCreditCardResult>>
{
    public async Task<ErrorOr<CancelCreditCardResult>> Handle(CancelCreditCardCommand request, CancellationToken cancellationToken)
    {
        var creditCard = await repository.GetCreditCardByIdAsync(request.CreditCardId, cancellationToken);
        if (creditCard is null) return Error.NotFound("CreditCard.NotFound", "Credit card not found");
        
        creditCard.Cancel();

        await repository.UpdateCreditCardAsync(creditCard, cancellationToken);

        return new CancelCreditCardResult(creditCard);
    }
}