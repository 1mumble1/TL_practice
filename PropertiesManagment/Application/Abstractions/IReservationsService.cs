using Application.Contracts;

namespace Application.Abstractions;

public interface IReservationsService
{
    Task<Guid> CreateReservation(
        Guid propertyPublicId,
        Guid roomTypePublicId,
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber );
    Task DeleteReservation( Guid id );
    Task<IReadOnlyList<ReservationDto>> GetAllReservations(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber );
    Task<ReservationDto?> GetReservationById( Guid id );
    Task<List<PropertyWithRoomTypesDto>> SearchAvailableReservations(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice );
}