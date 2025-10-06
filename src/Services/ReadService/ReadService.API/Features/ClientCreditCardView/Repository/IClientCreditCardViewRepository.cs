using ReadService.API.Features.ClientCreditCardView.Models;

namespace ReadService.API.Features.ClientCreditCardView.Repository;

public interface IClientCreditCardViewRepository
{
    Task<ClientCreditCardViewDocument?> FindCreditCardViewByClientIdAsync(Guid clientId);
    Task UpsertClientAsync(Guid clientId, string name, DateTime birthDate);
    Task UpsertCreditCardAsync(Guid clientId);
}