using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;

namespace Application.Services;

public class PropertiesService : IPropertiesService
{
    private readonly IPropertiesRepository _propertiesRepository;

    public PropertiesService( IPropertiesRepository propertiesRepository )
    {
        _propertiesRepository = propertiesRepository;
    }

    public async Task<List<Property>> GetAllProperties()
    {
        List<Property> properties = await _propertiesRepository.GetAll();
        return properties;
    }

    public async Task<Property?> GetPropertyById( Guid id )
    {
        Property? property = await _propertiesRepository.GetById( id );
        return property;
    }

    public async Task<Guid> CreateProperty(
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        try
        {
            Property property = new(
                name,
                country,
                city,
                address,
                latitude,
                longitude );
            var result = await _propertiesRepository.Create( property );
            return result;
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"Error: {ex.Message}" );
        }
    }

    public async Task<Guid> UpdateProperty(
        Guid id,
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        try
        {
            Property property = new(
                id,
                name,
                country,
                city,
                address,
                latitude,
                longitude );
            var result = await _propertiesRepository.Update( property );
            return result;
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"Error: {ex.Message}" );
        }
    }

    public async Task<Guid> DeleteProperty( Guid id )
    {
        return await _propertiesRepository.Delete( id );
    }
}
