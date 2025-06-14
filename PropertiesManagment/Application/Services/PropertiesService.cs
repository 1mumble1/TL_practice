using Application.Abstractions;
using Application.Contracts;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.Services;

public class PropertiesService : IPropertiesService
{
    private readonly IPropertiesRepository _propertiesRepository;

    public PropertiesService( IPropertiesRepository propertiesRepository )
    {
        _propertiesRepository = propertiesRepository;
    }

    public async Task<IReadOnlyList<PropertyDto>> GetAllProperties()
    {
        IReadOnlyList<Property> properties = await _propertiesRepository.GetAll();
        return properties
            .Select( p => new PropertyDto(
                p.PublicId,
                p.Name,
                p.Country,
                p.City,
                p.Address,
                p.Latitude,
                p.Longitude ) )
            .ToList();
    }

    public async Task<PropertyDto?> GetPropertyById( Guid id )
    {
        Property? property = await _propertiesRepository.GetById( id );
        if ( property is null )
        {
            return null;
        }

        return new PropertyDto(
            property.PublicId,
            property.Name,
            property.Country,
            property.City,
            property.Address,
            property.Longitude,
            property.Latitude );
    }

    public async Task<Guid> CreateProperty(
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        Property property = new(
            name,
            country,
            city,
            address,
            latitude,
            longitude );

        return await _propertiesRepository.Create( property );
    }

    public async Task UpdateProperty(
        Guid id,
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        Property? property = await _propertiesRepository.GetById( id );

        if ( property is null )
        {
            throw new ArgumentException( $"Not found property with id: {id}" );
        }

        property.Update(
            name,
            country,
            city,
            address,
            latitude,
            longitude );

        await _propertiesRepository.Update( property );
    }

    public async Task DeleteProperty( Guid id )
    {
        await _propertiesRepository.Delete( id );
    }
}
