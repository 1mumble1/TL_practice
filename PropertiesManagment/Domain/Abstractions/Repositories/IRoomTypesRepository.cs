using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IRoomTypesRepository
{
    Task<Guid> Create( RoomType roomType );
    Task Delete( Guid id );
    Task<IReadOnlyList<RoomType>> GetAllByPropertyId( int propertyId );
    Task<RoomType?> GetById( Guid id );
    Task Update( RoomType roomType );
    Task<Property?> GetPropertyById( Guid propertyId );
}