using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;

namespace Application.Services;

public class ReservationsService : IReservationsService
{
    private readonly IReservationsRepository _reservationsRepository;

    public ReservationsService( IReservationsRepository reservationsRepository )
    {
        _reservationsRepository = reservationsRepository;
    }

    public async Task<List<Property>> SearchAvailableReservations(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice )
    {
        List<Property> result = await _reservationsRepository.SearchAvailable(
            city,
            arrivalDate,
            departureDate,
            guests,
            maxDailyPrice );
        return result;
    }

    public async Task<Guid> CreateReservation(
        Guid propertyId,
        Guid roomTypeId,
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber )
    {
        try
        {
            if ( !await _reservationsRepository.ExistsProperty( propertyId ) )
            {
                throw new ArgumentException( $"Cannot found property with id {propertyId}" );
            }
            if ( !await _reservationsRepository.ExistsRoomType( roomTypeId ) )
            {
                throw new ArgumentException( $"Cannot found property with id {roomTypeId}" );
            }
            string currency = await _reservationsRepository.GetRoomTypeCurrency( roomTypeId );
            decimal dailyPrice = await _reservationsRepository.GetRoomTypeDailyPrice( roomTypeId );
            decimal total = CalculateTotal( dailyPrice, arrivalDate, arrivalTime, departureDate, departureTime );

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

            Guid result = await _reservationsRepository.Create( reservation );

            return result;
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( ex.Message );
        }
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

        decimal total = ( departure - arrival ).Days * dailyPrice;
        return total;
    }

    public async Task<List<Reservation>> GetAllReservations(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber )
    {
        return await _reservationsRepository.GetAll(
            propertyId,
            roomTypeId,
            arrivalDate,
            departureDate,
            guestName,
            guestPhoneNumber );
    }

    public async Task<Reservation?> GetReservationById( Guid id )
    {
        return await _reservationsRepository.GetById( id );
    }

    public async Task<Guid> DeleteReservation( Guid id )
    {
        return await _reservationsRepository.Delete( id );
    }
}
