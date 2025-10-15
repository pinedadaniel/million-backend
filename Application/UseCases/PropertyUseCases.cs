using million_backend.Application.DTOs;
using million_backend.Application.Interfaces;
using million_backend.Domain.Entities;
using million_backend.Domain.Interfaces;

namespace million_backend.Application.UseCases;

public class PropertyUseCases : IPropertyUseCases
{
    private readonly IPropertyRepository _propertyRepository;

    public PropertyUseCases(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<PropertiesResponseDto> GetPropertiesSeparatedAsync(PropertyFilterDto filter)
    {
        var (available, availableCount, sold, soldCount) = await _propertyRepository.GetPropertiesSeparatedAsync(
            filter.Page, 
            filter.PageSize, 
            filter.Name, 
            filter.Address, 
            filter.MinPrice, 
            filter.MaxPrice);

        return new PropertiesResponseDto
        {
            Available = new PagedResultDto<PropertyDto>
            {
                Data = available.Select(MapToDto),
                TotalCount = availableCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            },
            Sold = new PagedResultDto<PropertyDto>
            {
                Data = sold.Select(MapToDto),
                TotalCount = soldCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            },
            TotalAvailable = availableCount,
            TotalSold = soldCount,
            GrandTotal = availableCount + soldCount
        };
    }

    private static PropertyDto MapToDto(Property property)
    {
        return new PropertyDto
        {
            Id = property.Id,
            OwnerId = property.OwnerId,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            Image = property.Image,
            Description = property.Description,
            Sold = property.Sold
        };
    }
}
