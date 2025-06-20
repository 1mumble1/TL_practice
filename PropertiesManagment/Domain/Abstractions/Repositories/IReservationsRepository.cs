using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IReservationsRepository
{
    Task<Guid> Create( Reservation reservation );
    Task Delete( Guid id );
    Task<IReadOnlyList<Reservation>> GetAll(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber );
    Task<Reservation?> GetById( Guid id );
    Task<Property?> GetPropertyByPublicId( Guid propertyPublicId );
    Task<RoomType?> GetRoomTypeByPublicId( Guid roomTypePublicId );
    Task<IReadOnlyList<Property>> SearchAvailableProperties(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate );
    Task<IReadOnlyList<RoomType>> SearchAvailableRoomTypes(
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice );
    Task<IEnumerable<int>> GetAvailablePropertyIds(
        DateOnly arrivalDate,
        DateOnly departureDate );
    Task<IReadOnlyDictionary<int, int>> GetAvailableRoomTypesWithCounts(
        DateOnly arrivalDate,
        DateOnly departureDate );
}