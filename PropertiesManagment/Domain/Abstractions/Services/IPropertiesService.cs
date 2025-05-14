using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IPropertiesService
{
    Task<Guid> CreateProperty( string name, string country, string city, string address, decimal latitude, decimal longitude );
    Task<Guid> DeleteProperty( Guid id );
    Task<List<Property>> GetAllProperties();
    Task<Property?> GetPropertyById( Guid id );
    Task<Guid> UpdateProperty( Guid id, string name, string country, string city, string address, decimal latitude, decimal longitude );
}