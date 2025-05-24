using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IRoomTypesService
{
    Task<Guid> CreateRoomType(
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms );
    Task<Guid> DeleteRoomType( Guid id );
    Task<List<RoomType>> GetAllRoomTypesByPropertyId( Guid propertyId );
    Task<RoomType?> GetRoomTypeById( Guid id );
    Task<Guid> UpdateRoomType(
        Guid id,
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms );
}