using BuildingBlocks.CQRS;
using CreditCard.API.CreditCard.Persistence;

namespace CreditCard.API.CreditCard.BlockCreditCard;

public record BlockCreditCardCommand(Guid CreditCardId) : ICommand<ErrorOr<BlockCreditCardResult>>;

public record BlockCreditCardResult(Models.CreditCard CreditCard);

public class BlockCreditCardCommandHandler(ICreditCardRepository repository)
    : ICommandHandler<BlockCreditCardCommand, ErrorOr<BlockCreditCardResult>>
{
    public async Task<ErrorOr<BlockCreditCardResult>> Handle(BlockCreditCardCommand command,
        CancellationToken cancellationToken)
    {
        var creditCard = await repository.GetCreditCardByIdAsync(command.CreditCardId, cancellationToken);

        if (creditCard is null) return Error.NotFound("CreditCard.NotFound", "Credit Card not found");

        creditCard.Block();

        await repository.UpdateCreditCardAsync(creditCard, cancellationToken);

        return new BlockCreditCardResult(creditCard);
    }
}