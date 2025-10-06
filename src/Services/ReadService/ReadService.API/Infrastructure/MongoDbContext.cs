using MongoDB.Driver;

namespace ReadService.API.Infrastructure;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"];
        var databaseName = configuration["MongoDb:DatabaseName"];

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        
        CreateCollections();
    }
    
    private void CreateCollections()
    {
        var existingCollections = _database.ListCollectionNames().ToList();

        if (!existingCollections.Contains("Clients"))
            _database.CreateCollection("Clients");

        if (!existingCollections.Contains("Proposals"))
            _database.CreateCollection("Proposals");

        if (!existingCollections.Contains("CreditCards"))
            _database.CreateCollection("CreditCards");

        if (!existingCollections.Contains("ClientCreditCardView"))
            _database.CreateCollection("ClientCreditCardView");
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoDatabase Database => _database;
}