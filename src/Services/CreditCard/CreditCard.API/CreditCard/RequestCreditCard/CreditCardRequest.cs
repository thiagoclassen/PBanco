namespace CreditCard.API.CreditCard.RequestCreditCard;

public class CreditCardRequest
{
    public required Guid ClientId { get; set; }
    public required Guid ProposalId { get; set; }
    public required int ExpensesLimit { get; set; }
    public required string CardProvider { get; set; }
}