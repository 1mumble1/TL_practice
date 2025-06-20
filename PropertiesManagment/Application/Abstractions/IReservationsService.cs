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
    Task<IReadOnlyList<ReservationDto>> GetAllReservations( FilterParamsQuery filterParamsQuery );
    Task<ReservationDto?> GetReservationById( Guid id );
    Task<IReadOnlyList<PropertyWithRoomTypesDto>> SearchAvailableReservations( SearchParamsQuery searchParamsQuery );
}