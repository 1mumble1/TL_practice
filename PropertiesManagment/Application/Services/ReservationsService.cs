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
        if ( searchParamsQuery.ArrivalDate.HasValue && !searchParamsQuery.DepartureDate.HasValue ||
            !searchParamsQuery.ArrivalDate.HasValue && searchParamsQuery.DepartureDate.HasValue )
        {
            throw new ArgumentException( "Cannot search with only one date" );
        }

        IReadOnlyList<Property> properties = await _reservationsRepository.SearchAvailableProperties(
            searchParamsQuery.City,
            searchParamsQuery.ArrivalDate,
            searchParamsQuery.DepartureDate );

        IReadOnlyList<RoomType> roomTypes = await _reservationsRepository.SearchAvailableRoomTypes(
            searchParamsQuery.ArrivalDate,
            searchParamsQuery.DepartureDate,
            searchParamsQuery.Guests,
            searchParamsQuery.MaxDailyPrice );

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
        Property? property = await _reservationsRepository.GetPropertyByPublicId( propertyPublicId )
            ?? throw new ArgumentException( $"Cannot found property with id {propertyPublicId}" );

        RoomType? roomType = await _reservationsRepository.GetRoomTypeByPublicId( roomTypePublicId )
            ?? throw new ArgumentException( $"Cannot found room type with id {roomTypePublicId}" );

        if ( !( await _reservationsRepository
                .GetAvailablePropertyIds( arrivalDate, departureDate ) )
                .Contains( property.Id ) ||
            !( await _reservationsRepository
                .GetAvailableRoomTypesWithCounts( arrivalDate, departureDate ) )
                .ContainsKey( roomType.Id ) )
        {
            throw new InvalidOperationException( "The reservation on this dates already exists" );
        }

        string currency = roomType.Currency;
        decimal dailyPrice = roomType.DailyPrice;
        decimal total = CalculateTotal(
            dailyPrice,
            arrivalDate,
            arrivalTime,
            departureDate,
            departureTime );

        Reservation reservation = new(
            property.Id,
            roomType.Id,
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

    public async Task<IReadOnlyList<ReservationDto>> GetAllReservations( FilterParamsQuery filterParamsQuery )
    {
        IReadOnlyList<Reservation> reservations = await _reservationsRepository.GetAll(
            filterParamsQuery.PropertyId,
            filterParamsQuery.RoomTypeId,
            filterParamsQuery.ArrivalDate,
            filterParamsQuery.DepartureDate,
            filterParamsQuery.GuestName,
            filterParamsQuery.GuestPhoneNumber );

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
