using million_backend.Application.DTOs;

namespace million_backend.Application.Interfaces;

public interface IPropertyUseCases
{
    Task<PropertiesResponseDto> GetPropertiesSeparatedAsync(PropertyFilterDto filter);
}
