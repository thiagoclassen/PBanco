namespace ReadService.API.Features.ClientCreditCardView.Repository;

public interface IClientCreditCardViewRepository
{
    Task UpsertClientAsync(Guid clientId, string name, DateTime birthDate);
    Task UpsertCreditCardAsync(Guid clientId);
}