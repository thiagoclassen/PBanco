using BuildingBlocks.Domain.Exceptions;

namespace Clients.API.Client.Exceptions;

public class CpfAlreadyExistsExceptions : DomainException
{
    public CpfAlreadyExistsExceptions(string cpf)
        : base($"Client with CPF {cpf} already exists.") { }
}