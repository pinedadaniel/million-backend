using MongoDB.Driver;
using million_backend.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace million_backend.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<PropertyDocument> Properties => _database.GetCollection<PropertyDocument>("properties");
}
