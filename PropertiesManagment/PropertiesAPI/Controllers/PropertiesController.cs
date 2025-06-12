using Application.Abstractions;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using PropertiesAPI.Contracts.Property;

namespace PropertiesAPI.Controllers;

[ApiController]
[Route( "api/properties" )]
public class PropertiesController : ControllerBase
{
    private readonly IPropertiesService _propertiesService;

    public PropertiesController( IPropertiesService propertiesService )
    {
        _propertiesService = propertiesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProperties()
    {
        IReadOnlyList<PropertyDto> properties = await _propertiesService.GetAllProperties();

        IReadOnlyList<PropertyResponse> propertiesResponse = properties
            .Select( p => new PropertyResponse(
                p.Id,
                p.Name,
                p.Country,
                p.City,
                p.Address,
                p.Latitude,
                p.Longitude ) )
            .ToList();

        return Ok( propertiesResponse );
    }

    [HttpGet( "{id:guid}" )]
    public async Task<IActionResult> GetPropertyById( [FromRoute] Guid id )
    {
        PropertyDto? property = await _propertiesService.GetPropertyById( id );
        if ( property is null )
        {
            return NotFound();
        }

        PropertyResponse propertyResponse = new(
            property.Id,
            property.Name,
            property.Country,
            property.City,
            property.Address,
            property.Latitude,
            property.Longitude );

        return Ok( propertyResponse );
    }

    [HttpPost]
    public async Task<IActionResult> CreateProperty( [FromBody] CreatePropertyRequest createPropertyRequest )
    {
        try
        {
            Guid result = await _propertiesService.CreateProperty(
                createPropertyRequest.Name,
                createPropertyRequest.Country,
                createPropertyRequest.City,
                createPropertyRequest.Address,
                createPropertyRequest.Latitude,
                createPropertyRequest.Longitude );
            return Ok( result );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpPut( "{id:guid}" )]
    public async Task<IActionResult> UpdateProperty( [FromRoute] Guid id, [FromBody] UpdatePropertyRequest updatePropertyRequest )
    {
        try
        {
            await _propertiesService.UpdateProperty(
                id,
                updatePropertyRequest.Name,
                updatePropertyRequest.Country,
                updatePropertyRequest.City,
                updatePropertyRequest.Address,
                updatePropertyRequest.Latitude,
                updatePropertyRequest.Longitude );

            return Ok();
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpDelete( "{id:guid}" )]
    public async Task<IActionResult> DeleteProperty( [FromRoute] Guid id )
    {
        await _propertiesService.DeleteProperty( id );
        return Ok();
    }
}
