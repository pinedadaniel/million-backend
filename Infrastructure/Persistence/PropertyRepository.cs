using MongoDB.Driver;
using million_backend.Domain.Entities;
using million_backend.Domain.Interfaces;

namespace million_backend.Infrastructure.Persistence;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<PropertyDocument> _properties;

    public PropertyRepository(MongoDbContext context)
    {
        _properties = context.Properties;
    }

    public async Task<(IEnumerable<Property> Available, int AvailableCount, IEnumerable<Property> Sold, int SoldCount)> GetPropertiesSeparatedAsync(int page, int pageSize, string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var filterBuilder = Builders<PropertyDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(name))
        {
            filterBuilder &= Builders<PropertyDocument>.Filter.Regex(x => x.Name, 
                new MongoDB.Bson.BsonRegularExpression(name, "i"));
        }

        if (!string.IsNullOrEmpty(address))
        {
            filterBuilder &= Builders<PropertyDocument>.Filter.Regex(x => x.Address, 
                new MongoDB.Bson.BsonRegularExpression(address, "i"));
        }

        if (minPrice.HasValue)
        {
            filterBuilder &= Builders<PropertyDocument>.Filter.Gte(x => x.Price, minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filterBuilder &= Builders<PropertyDocument>.Filter.Lte(x => x.Price, maxPrice.Value);
        }

        var availableFilter = filterBuilder & Builders<PropertyDocument>.Filter.Eq(x => x.Sold, false);
        var availableCount = await _properties.CountDocumentsAsync(availableFilter);
        
        var skip = (page - 1) * pageSize;
        var availableDocuments = await _properties
            .Find(availableFilter)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

        var soldFilter = Builders<PropertyDocument>.Filter.Eq(x => x.Sold, true);
        var soldCount = await _properties.CountDocumentsAsync(soldFilter);
        
        var soldDocuments = await _properties
            .Find(soldFilter)
            .Skip(skip)
            .Limit(10)
            .ToListAsync();

        return (
            availableDocuments.Select(MapToDomain), 
            (int)availableCount,
            soldDocuments.Select(MapToDomain), 
            (int)soldCount
        );
    }

    private static Property MapToDomain(PropertyDocument document)
    {
        return new Property
        {
            Id = document.Id,
            OwnerId = document.OwnerId,
            Name = document.Name,
            Address = document.Address,
            Price = document.Price,
            Image = document.Image,
            Description = document.Description,
            Sold = document.Sold,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
    }
}
