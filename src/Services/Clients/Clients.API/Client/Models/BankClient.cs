using System.Text.Json;
using BuildingBlocks.Domain;
using BuildingBlocks.Events.Client;

namespace Clients.API.Client.Models;

public class BankClient : AggregateRoot
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
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

        client.AddDomainEvent(new ClientCreatedEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ClientId = client.Id,
        });
        
        client.AddDomainEvent(client.CreatePropagateEvent());

        return client;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        
        AddDomainEvent(new ClientDeletedEvent()
        {
            EventId = Guid.NewGuid(),
            ClientId = Id,
            OccurredOn = DateTime.UtcNow
        });
        
        AddDomainEvent(CreatePropagateEvent());
    }
    
    private ClientPropagateEvent CreatePropagateEvent()
    {
        return new ClientPropagateEvent
        {
            EventId = Guid.NewGuid(),
            OccurredOn = DateTime.UtcNow,
            ClientId = Id,
            ClientName = Name,
            ClientEmail = Email,
            ClientCPF = CPF,
            ClientBirthDate = BirthDate,
            ClientIsDeleted = IsDeleted,
            ClientDeletedAt = DeletedAt,
            ClientCreatedAt = CreatedAt,
            ClientUpdatedAt = UpdatedAt
        };
    }
}