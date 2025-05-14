using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IRoomTypesRepository
{
    Task<Guid> Create( RoomType roomType );
    Task<Guid> Delete( Guid id );
    Task<List<RoomType>> GetAllByPropertyId( Guid propertyId );
    Task<RoomType?> GetById( Guid id );
    Task<Guid> Update( RoomType roomType );
}