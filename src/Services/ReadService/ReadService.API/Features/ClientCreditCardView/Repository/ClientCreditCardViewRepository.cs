using MongoDB.Driver;
using ReadService.API.Features.ClientCreditCardView.Models;
using ReadService.API.Features.CreditCards.Models;
using ReadService.API.Infrastructure;

namespace ReadService.API.Features.ClientCreditCardView.Repository;

public class ClientCreditCardViewRepository(MongoDbContext context) : IClientCreditCardViewRepository
{
    private readonly IMongoCollection<ClientCreditCardViewDocument> _collection =
        context.GetCollection<ClientCreditCardViewDocument>("ClientCreditCardView");

    private readonly IMongoCollection<CreditCardDocument> _creditCards =
        context.GetCollection<CreditCardDocument>("CreditCards");

    public async Task UpsertClientAsync(Guid clientId, string name, DateTime birthDate)
    {
        var creditCards = await _creditCards
            .Find(c => c.ClientId == clientId)
            .Project(c => new CardSummary { Number = c.Number, Provider = c.CardProvider, Status = c.CardStatus })
            .ToListAsync();

        var view = new ClientCreditCardViewDocument
        {
            ClientId = clientId,
            Name = name,
            BirthDate = birthDate,
            CreditCards = creditCards
        };

        await _collection.ReplaceOneAsync(
            filter: Builders<ClientCreditCardViewDocument>.Filter.Eq(v => v.ClientId, clientId),
            replacement: view,
            options: new ReplaceOptions { IsUpsert = true });
    }

    public async Task UpsertCreditCardAsync(Guid clientId)
    {
        var existingView = await _collection
            .Find(v => v.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (existingView is null) return;

        var creditCards = await _creditCards
            .Find(c => c.ClientId == clientId)
            .Project(c => new CardSummary { Number = c.Number, Provider = c.CardProvider, Status = c.CardStatus })
            .ToListAsync();

        existingView.CreditCards = creditCards;

        await _collection.ReplaceOneAsync(
            filter: Builders<ClientCreditCardViewDocument>.Filter.Eq(v => v.ClientId, clientId),
            replacement: existingView,
            options: new ReplaceOptions { IsUpsert = true });
    }
}