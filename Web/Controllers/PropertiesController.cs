using Microsoft.AspNetCore.Mvc;
using million_backend.Application.DTOs;
using million_backend.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace million_backend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyUseCases _propertyUseCases;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(IPropertyUseCases propertyUseCases, ILogger<PropertiesController> logger)
    {
        _propertyUseCases = propertyUseCases;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PropertiesResponseDto>> GetProperties(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int page = 1,
        [FromQuery] [Range(1, 10)] int pageSize = 3)
    {
        try
        {
            var filter = new PropertyFilterDto
            {
                Name = name,
                Address = address,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = Math.Max(1, page),
                PageSize = Math.Clamp(pageSize, 1, 10)
            };

            var result = await _propertyUseCases.GetPropertiesSeparatedAsync(filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties");
            return StatusCode(500, new { message = "Internal Server Error" });
        }
    }
}
