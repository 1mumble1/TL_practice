using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;

namespace Application.Services;

public class RoomTypesService : IRoomTypesService
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    public RoomTypesService( IRoomTypesRepository roomTypesRepository )
    {
        _roomTypesRepository = roomTypesRepository;
    }

    public async Task<List<RoomType>> GetAllRoomTypesByPropertyId( Guid propertyId )
    {
        List<RoomType> roomTypes = await _roomTypesRepository.GetAllByPropertyId( propertyId );
        return roomTypes;
    }

    public async Task<RoomType?> GetRoomTypeById( Guid id )
    {
        RoomType? roomType = await _roomTypesRepository.GetById( id );
        return roomType;
    }

    public async Task<Guid> CreateRoomType(
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms )
    {
        try
        {
            RoomType roomType = new(
                propertyId,
                name,
                dailyPrice,
                currency,
                minPersonCount,
                maxPersonCount,
                services,
                amenities,
                availableRooms );
            var result = await _roomTypesRepository.Create( roomType );
            return result;
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"Error: {ex.Message}" );
        }
    }

    public async Task<Guid> UpdateRoomType(
        Guid id,
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms )
    {
        try
        {
            RoomType roomType = new(
                id,
                propertyId,
                name,
                dailyPrice,
                currency,
                minPersonCount,
                maxPersonCount,
                services,
                amenities,
                availableRooms );
            var result = await _roomTypesRepository.Update( roomType );
            return result;
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"Error: {ex.Message}" );
        }
    }

    public async Task<Guid> DeleteRoomType( Guid id )
    {
        return await _roomTypesRepository.Delete( id );
    }
}
