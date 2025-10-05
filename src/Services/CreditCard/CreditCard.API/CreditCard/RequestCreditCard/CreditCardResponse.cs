namespace CreditCard.API.CreditCard.RequestCreditCard;

public record CreditCardResponse(
    Guid CreditCardId,
    Guid ClientId,
    Guid ProposalId,
    int CreditCardNumber,
    string ExpensesLimit,
    string CreditCardProvider,
    string CreditCardStatus,
    DateTime CreatedAt,
    DateTime UpdatedAt);