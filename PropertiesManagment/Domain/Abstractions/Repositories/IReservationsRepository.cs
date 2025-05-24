using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IReservationsRepository
{
    Task<Guid> Create( Reservation reservation );
    Task<Guid> Delete( Guid id );
    Task<List<Reservation>> GetAll(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber );
    Task<Reservation?> GetById( Guid id );
    Task<List<Property>> SearchAvailable(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice );
    Task<bool> ExistsProperty( Guid propertyId );
    Task<bool> ExistsRoomType( Guid roomTypeId );
    Task<decimal> GetRoomTypeDailyPrice( Guid roomTypeId );
    Task<string> GetRoomTypeCurrency( Guid roomTypeId );
}