using Application.Abstractions;
using Application.Contracts;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.Services;

public class RoomTypesService : IRoomTypesService
{
    private readonly IRoomTypesRepository _roomTypesRepository;

    public RoomTypesService( IRoomTypesRepository roomTypesRepository )
    {
        _roomTypesRepository = roomTypesRepository;
    }

    public async Task<IReadOnlyList<RoomTypeDto>> GetAllRoomTypesByPropertyId( Guid propertyId )
    {
        Property? property = await _roomTypesRepository.GetPropertyById( propertyId );

        if ( property is null )
        {
            throw new ArgumentException( $"Not found property with id: {propertyId}" );
        }

        IReadOnlyList<RoomType> roomTypes = await _roomTypesRepository.GetAllByPropertyId( property.Id );

        return roomTypes
            .Select( rt => new RoomTypeDto(
                rt.PublicId,
                rt.Property.PublicId,
                rt.Name,
                rt.DailyPrice,
                rt.Currency,
                rt.MinPersonCount,
                rt.MaxPersonCount,
                rt.Services,
                rt.Amenities,
                rt.AvailableRooms ) )
            .ToList();
    }

    public async Task<RoomTypeDto?> GetRoomTypeById( Guid id )
    {
        RoomType? roomType = await _roomTypesRepository.GetById( id );
        if ( roomType is null )
        {
            return null;
        }

        return new RoomTypeDto(
            roomType.PublicId,
            roomType.Property.PublicId,
            roomType.Name,
            roomType.DailyPrice,
            roomType.Currency,
            roomType.MinPersonCount,
            roomType.MaxPersonCount,
            roomType.Services,
            roomType.Amenities,
            roomType.AvailableRooms );
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
        Property? property = await _roomTypesRepository.GetPropertyById( propertyId );

        if ( property is null )
        {
            throw new ArgumentException( $"Not found property with id: {propertyId}" );
        }

        RoomType roomType = new(
            property.Id,
            name,
            dailyPrice,
            currency,
            minPersonCount,
            maxPersonCount,
            services,
            amenities,
            availableRooms );

        return await _roomTypesRepository.Create( roomType );
    }

    public async Task UpdateRoomType(
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
        RoomType? roomType = await _roomTypesRepository.GetById( id );

        if ( roomType is null )
        {
            throw new ArgumentException( $"Not found room type with id: {id}" );
        }

        Property? property = await _roomTypesRepository.GetPropertyById( propertyId );

        if ( property is null )
        {
            throw new ArgumentException( $"Not found property with id: {propertyId}" );
        }

        roomType.Update(
            property.Id,
            name,
            dailyPrice,
            currency,
            minPersonCount,
            maxPersonCount,
            services,
            amenities,
            availableRooms );

        await _roomTypesRepository.Update( roomType );
    }

    public async Task DeleteRoomType( Guid id )
    {
        await _roomTypesRepository.Delete( id );
    }
}
