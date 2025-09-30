using BuildingBlocks.Domain;
using BuildingBlocks.Events;
using BuildingBlocks.Events.Client;

namespace Clients.API.Client.Models;

public class BankClient : AggregateRoot
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public static BankClient Create(string name, string email, string cpf, DateTime birthDate)
    {
        var client = new BankClient
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            CPF = cpf,
            BirthDate = birthDate,
        };
        
        client.AddDomainEvent(new ClientCreated
        {
            Id = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ClientId = client.Id,
        });

        return client;
    }
}
