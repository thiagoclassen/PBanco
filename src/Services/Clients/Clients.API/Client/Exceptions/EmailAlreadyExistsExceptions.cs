using BuildingBlocks.Domain.Exceptions;

namespace Clients.API.Client.Exceptions;

public class EmailAlreadyExistsExceptions : DomainException
{
    public EmailAlreadyExistsExceptions(string email)
        : base($"Client with email {email} already exists.") { }
}