using Domain.Abstractions.Contracts;
using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IReservationsService
{
    Task<Guid> CreateReservation(
        Guid propertyId,
        Guid roomTypeId,
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber );
    Task<Guid> DeleteReservation( Guid id );
    Task<List<Reservation>> GetAllReservations(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber );
    Task<Reservation?> GetReservationById( Guid id );
    Task<List<PropertyWithRoomTypesDto>> SearchAvailableReservations(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice );
}