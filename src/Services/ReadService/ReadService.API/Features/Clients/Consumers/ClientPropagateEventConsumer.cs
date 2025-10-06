using BuildingBlocks.Domain.Events.Client;
using MassTransit;
using ReadService.API.Features.ClientCreditCardView.Repository;
using ReadService.API.Features.Clients.Models;
using ReadService.API.Features.Clients.Repository;

namespace ReadService.API.Features.Clients.Consumers;

public class ClientPropagateEventConsumer(
    IClientRepository clientRepository,
    IClientCreditCardViewRepository clientCreditCardViewRepository)
    : IConsumer<ClientPropagateEvent>
{
    public async Task Consume(ConsumeContext<ClientPropagateEvent> context)
    {
        var @event = context.Message;

        var clientDocument = new ClientDocument
        {
            Id = @event.ClientId,
            Name = @event.ClientName,
            Email = @event.ClientEmail,
            CPF = @event.ClientCPF,
            BirthDate = @event.ClientBirthDate,
            IsDeleted = @event.ClientIsDeleted,
            DeletedAt = @event.ClientDeletedAt,
            CreatedAt = @event.ClientCreatedAt,
            UpdatedAt = @event.ClientUpdatedAt
        };

        await clientRepository.InsertOrUpdateAsync(clientDocument);
        await clientCreditCardViewRepository.UpsertClientAsync(
            @event.ClientId,
            @event.ClientName,
            @event.ClientBirthDate);
    }
}