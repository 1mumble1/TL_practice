using Application.Contracts;

namespace Application.Abstractions;

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
    Task DeleteRoomType( Guid id );
    Task<IReadOnlyList<RoomTypeDto>> GetAllRoomTypesByPropertyId( Guid propertyId );
    Task<RoomTypeDto?> GetRoomTypeById( Guid id );
    Task UpdateRoomType(
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