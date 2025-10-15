using million_backend.Domain.Entities;

namespace million_backend.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<(IEnumerable<Property> Available, int AvailableCount, IEnumerable<Property> Sold, int SoldCount)> GetPropertiesSeparatedAsync(int page, int pageSize, string? name, string? address, decimal? minPrice, decimal? maxPrice);
}
