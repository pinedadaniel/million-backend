namespace million_backend.Application.DTOs;

public class PropertiesResponseDto
{
    public PagedResultDto<PropertyDto> Available { get; set; } = new();
    public PagedResultDto<PropertyDto> Sold { get; set; } = new();
    public int TotalAvailable { get; set; }
    public int TotalSold { get; set; }
    public int GrandTotal { get; set; }
}

