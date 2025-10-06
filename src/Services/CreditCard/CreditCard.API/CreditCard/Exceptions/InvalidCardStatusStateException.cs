using BuildingBlocks.Domain.Exceptions;
using CreditCard.API.CreditCard.Models;

namespace CreditCard.API.CreditCard.Exceptions;

public class InvalidCardStatusStateException : DomainException
{
    public InvalidCardStatusStateException(string newStatus, string oldStatus)
        : base($"Cannot transition to {newStatus} from {oldStatus}.") { }
    
    public InvalidCardStatusStateException(string status)
        : base($"The card status '{status}' is not a valid state transition.") { }
}