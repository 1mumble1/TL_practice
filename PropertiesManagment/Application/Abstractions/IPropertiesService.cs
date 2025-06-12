using Application.Contracts;

namespace Application.Abstractions;

public interface IPropertiesService
{
    Task<Guid> CreateProperty(
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude );
    Task DeleteProperty( Guid id );
    Task<IReadOnlyList<PropertyDto>> GetAllProperties();
    Task<PropertyDto?> GetPropertyById( Guid id );
    Task UpdateProperty(
        Guid id,
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude );
}