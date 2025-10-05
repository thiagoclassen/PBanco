using BuildingBlocks.Domain.Shared;
using CreditCard.API.CreditCard.Models;
using CreditCard.API.CreditCard.RequestCreditCard;

namespace CreditCard.API.CreditCard;

public static class CreditCardMapping
{
    public static RequestCreditCardCommand MapToCreditCardCommand(this CreditCardRequest request)
    {
        return new RequestCreditCardCommand(
            request.ClientId,
            request.ProposalId,
            new Money(request.ExpensesLimit),
            Enum.Parse<CardProvider>(request.CardProvider));
    }

    public static CreditCardResponse MapToCreditCardResponse(this Models.CreditCard request)
    {
        return new CreditCardResponse(
            request.Id,
            request.ClientId,
            request.ProposalId,
            request.Number,
            request.ExpensesLimit.ToString(),
            request.CardProvider.ToString(),
            request.CardStatus.ToString(),
            request.CreatedAt,
            request.UpdatedAt);
    }
}