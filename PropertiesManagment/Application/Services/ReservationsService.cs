using Application.Abstractions;
using Application.Contracts;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.Services;

public class ReservationsService : IReservationsService
{
    private readonly IReservationsRepository _reservationsRepository;

    public ReservationsService( IReservationsRepository reservationsRepository )
    {
        _reservationsRepository = reservationsRepository;
    }

    public async Task<IReadOnlyList<PropertyWithRoomTypesDto>> SearchAvailableReservations( SearchParamsQuery searchParamsQuery )
    {
        (string? city, DateOnly? arrivalDate, DateOnly? departureDate, int? guests, decimal? maxDailyPrice) = searchParamsQuery;

        if ( arrivalDate.HasValue && !departureDate.HasValue ||
            !arrivalDate.HasValue && departureDate.HasValue )
        {
            throw new ArgumentException( "Cannot search with only one date" );
        }

        IReadOnlyList<Property> properties = await _reservationsRepository.SearchAvailableProperties(
            city,
            arrivalDate,
            departureDate );

        IReadOnlyList<RoomType> roomTypes = await _reservationsRepository.SearchAvailableRoomTypes(
            arrivalDate,
            departureDate,
            guests,
            maxDailyPrice );

        return properties
            .Select( p => new PropertyWithRoomTypesDto
            (
                new PropertyDto(
                    p.PublicId,
                    p.Name,
                    p.Country,
                    p.City,
                    p.Address,
                    p.Latitude,
                    p.Longitude ),
                roomTypes
                    .Where( rt => rt.PropertyId == p.Id )
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
                    .ToList()
            ) )
            .ToList();
    }

    public async Task<Guid> CreateReservation(
        Guid propertyPublicId,
        Guid roomTypePublicId,
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber )
    {
        if ( !await _reservationsRepository.ExistsProperty( propertyPublicId ) )
        {
            throw new ArgumentException( $"Cannot found property with id {propertyPublicId}" );
        }

        if ( !await _reservationsRepository.ExistsRoomType( roomTypePublicId ) )
        {
            throw new ArgumentException( $"Cannot found room type with id {roomTypePublicId}" );
        }

        int propertyId = await _reservationsRepository.GetPropertyIdByPublicId( propertyPublicId );
        int roomTypeId = await _reservationsRepository.GetRoomTypeIdByPublicId( roomTypePublicId );

        if ( !( await _reservationsRepository
                .GetAvailablePropertyIds( arrivalDate, departureDate ) )
                .Contains( propertyId ) ||
            !( await _reservationsRepository
                .GetAvailableRoomTypesWithCounts( arrivalDate, departureDate ) )
                .ContainsKey( roomTypeId ) )
        {
            throw new InvalidOperationException( "The reservation on this dates already exists" );
        }

        string currency = await _reservationsRepository.GetRoomTypeCurrency( roomTypeId );
        decimal dailyPrice = await _reservationsRepository.GetRoomTypeDailyPrice( roomTypeId );
        decimal total = CalculateTotal(
            dailyPrice,
            arrivalDate,
            arrivalTime,
            departureDate,
            departureTime );

        Reservation reservation = new(
            propertyId,
            roomTypeId,
            arrivalDate,
            departureDate,
            arrivalTime,
            departureTime,
            guestName,
            guestPhoneNumber,
            currency,
            total );

        return await _reservationsRepository.Create( reservation );
    }

    private decimal CalculateTotal(
        decimal dailyPrice,
        DateOnly arrivalDate,
        TimeOnly arrivalTime,
        DateOnly departureDate,
        TimeOnly departureTime )
    {
        DateTime arrival = arrivalDate.ToDateTime( arrivalTime );
        DateTime departure = departureDate.ToDateTime( departureTime );

        return ( departure - arrival ).Days * dailyPrice;
    }

    public async Task<IReadOnlyList<ReservationDto>> GetAllReservations(
        FilterParamsQuery filterParamsQuery )
    {
        (Guid? propertyId, Guid? roomTypeId, DateOnly? arrivalDate, DateOnly? departureDate, string? guestName, string? guestPhoneNumber) = filterParamsQuery;

        IReadOnlyList<Reservation> reservations = await _reservationsRepository.GetAll(
            propertyId,
            roomTypeId,
            arrivalDate,
            departureDate,
            guestName,
            guestPhoneNumber );

        return reservations
            .Select( r => new ReservationDto(
                r.PublicId,
                r.Property.PublicId,
                r.RoomType.PublicId,
                r.ArrivalDate,
                r.DepartureDate,
                r.ArrivalTime,
                r.DepartureTime,
                r.GuestName,
                r.GuestPhoneNumber,
                r.Total,
                r.Currency ) )
            .ToList();
    }

    public async Task<ReservationDto?> GetReservationById( Guid id )
    {
        Reservation? reservation = await _reservationsRepository.GetById( id );

        if ( reservation is null )
        {
            return null;
        }

        return new ReservationDto(
            reservation.PublicId,
            reservation.Property.PublicId,
            reservation.RoomType.PublicId,
            reservation.ArrivalDate,
            reservation.DepartureDate,
            reservation.ArrivalTime,
            reservation.DepartureTime,
            reservation.GuestName,
            reservation.GuestPhoneNumber,
            reservation.Total,
            reservation.Currency );
    }

    public async Task DeleteReservation( Guid id )
    {
        await _reservationsRepository.Delete( id );
    }
}
