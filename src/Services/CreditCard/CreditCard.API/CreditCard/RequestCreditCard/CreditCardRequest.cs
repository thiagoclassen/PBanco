namespace CreditCard.API.CreditCard.RequestCreditCard;

public class CreditCardRequest
{
    public required Guid ClientId { get; set; }
    public required string CardProvider { get; set; }
}