using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationsRepository : IReservationsRepository
{
    private readonly PropertiesDbContext _dbContext;

    public ReservationsRepository( PropertiesDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Property>> SearchAvailableProperties(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate )
    {
        IQueryable<Property> query = _dbContext.Properties.AsQueryable();

        if ( !string.IsNullOrEmpty( city ) )
        {
            query = query.Where( p => p.City == city );
        }

        if ( arrivalDate.HasValue && departureDate.HasValue )
        {
            IReadOnlyDictionary<int, int> availableRoomTypes = await GetAvailableRoomTypesWithCounts( arrivalDate.Value, departureDate.Value );
            List<RoomType> allRoomTypes = await _dbContext.RoomTypes.ToListAsync();
            IEnumerable<int> propertyIdsWithAvailableRooms = allRoomTypes
                .Where( rt => availableRoomTypes.ContainsKey( rt.Id ) )
                .Select( rt => rt.PropertyId )
                .Distinct();

            query = query.Where( p => propertyIdsWithAvailableRooms.Contains( p.Id ) );
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<RoomType>> SearchAvailableRoomTypes(
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice )
    {
        IQueryable<RoomType> query = _dbContext.RoomTypes.AsQueryable();

        if ( guests.HasValue )
        {
            query = query.Where( rt => rt.MinPersonCount <= guests && rt.MaxPersonCount >= guests );
        }

        if ( maxDailyPrice.HasValue )
        {
            query = query.Where( rt => rt.DailyPrice <= maxDailyPrice );
        }

        List<RoomType> roomTypes = await query.ToListAsync();

        if ( !arrivalDate.HasValue || !departureDate.HasValue )
        {
            return roomTypes;
        }

        IReadOnlyDictionary<int, int> availableRoomCounts = await GetAvailableRoomTypesWithCounts( arrivalDate.Value, departureDate.Value );

        return roomTypes
            .Where( rt => availableRoomCounts.ContainsKey( rt.Id ) )
            .Select( rt =>
            {
                rt.Update(
                    rt.PropertyId,
                    rt.Name,
                    rt.DailyPrice,
                    rt.Currency,
                    rt.MinPersonCount,
                    rt.MaxPersonCount,
                    rt.Services,
                    rt.Amenities,
                    availableRoomCounts[ rt.Id ] );

                return rt;
            } )
            .Where( x => x.AvailableRooms > 0 )
            .ToList();
    }

    public async Task<IEnumerable<int>> GetAvailablePropertyIds( DateOnly arrivalDate, DateOnly departureDate )
    {
        IReadOnlyList<Reservation> conflictingReservations = await GetConflictingReservations( arrivalDate, departureDate );
        Dictionary<int, int> bookedRoomTypes = GetBookedRoomTypes( conflictingReservations );
        List<RoomType> allRoomTypes = await _dbContext.RoomTypes.ToListAsync();

        return allRoomTypes
            .Where( rt => !bookedRoomTypes.ContainsKey( rt.Id ) || bookedRoomTypes[ rt.Id ] < rt.AvailableRooms )
            .Select( rt => rt.PropertyId )
            .Distinct();
    }

    public async Task<IReadOnlyDictionary<int, int>> GetAvailableRoomTypesWithCounts(
        DateOnly arrivalDate,
        DateOnly departureDate )
    {
        IReadOnlyList<Reservation> conflictingReservations = await GetConflictingReservations( arrivalDate, departureDate );
        Dictionary<int, int> bookedRoomTypes = GetBookedRoomTypes( conflictingReservations );
        List<RoomType> allRoomTypes = await _dbContext.RoomTypes.ToListAsync();

        return allRoomTypes
            .ToDictionary(
                rt => rt.Id,
                rt => rt.AvailableRooms - ( bookedRoomTypes.TryGetValue( rt.Id, out var booked ) ? booked : 0 )
            )
            .Where( kv => kv.Value > 0 )
            .ToDictionary( kv => kv.Key, kv => kv.Value );
    }

    private async Task<IReadOnlyList<Reservation>> GetConflictingReservations( DateOnly arrivalDate, DateOnly departureDate )
    {
        return await _dbContext.Reservations
            .Where( r => r.ArrivalDate < departureDate && r.DepartureDate > arrivalDate )
            .ToListAsync();
    }

    private Dictionary<int, int> GetBookedRoomTypes( IReadOnlyList<Reservation> reservations )
    {
        return reservations
            .GroupBy( r => r.RoomTypeId )
            .ToDictionary( g => g.Key, g => g.Count() );
    }

    public async Task<Guid> Create( Reservation reservation )
    {
        await _dbContext.AddAsync( reservation );
        await _dbContext.SaveChangesAsync();
        return reservation.PublicId;
    }

    public async Task<IReadOnlyList<Reservation>> GetAll(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber )
    {
        List<Reservation> result = await _dbContext.Reservations
            .AsNoTracking()
            .Include( r => r.Property )
            .Include( r => r.RoomType )
            .ToListAsync();

        if ( propertyId.HasValue )
        {
            Property? property = _dbContext.Properties.FirstOrDefault( p => p.PublicId == propertyId.Value );
            if ( property is not null )
            {
                result = result.Where( r => r.PropertyId == property.Id ).ToList();
            }
        }

        if ( roomTypeId.HasValue )
        {
            RoomType? roomType = _dbContext.RoomTypes.FirstOrDefault( rt => rt.PublicId == roomTypeId.Value );
            if ( roomType is not null )
            {
                result = result.Where( r => r.RoomTypeId == roomType.Id ).ToList();
            }
        }

        if ( arrivalDate.HasValue )
        {
            result = result.Where( r => r.ArrivalDate == arrivalDate ).ToList();
        }

        if ( departureDate.HasValue )
        {
            result = result.Where( r => r.DepartureDate == departureDate ).ToList();
        }

        if ( guestName is not null )
        {
            result = result.Where( r => r.GuestName == guestName ).ToList();
        }

        if ( guestPhoneNumber is not null )
        {
            result = result.Where( r => r.GuestPhoneNumber == guestPhoneNumber ).ToList();
        }

        return result;
    }

    public async Task<Reservation?> GetById( Guid id )
    {
        return await _dbContext.Reservations
            .Include( r => r.Property )
            .Include( r => r.RoomType )
            .FirstOrDefaultAsync( r => r.PublicId == id );
    }

    public async Task Delete( Guid id )
    {
        await _dbContext.Reservations
            .Where( r => r.PublicId == id )
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsProperty( Guid propertyId )
    {
        return await _dbContext.Properties.FirstOrDefaultAsync( p => p.PublicId == propertyId ) is not null;
    }

    public async Task<bool> ExistsRoomType( Guid roomTypeId )
    {
        return await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.PublicId == roomTypeId ) is not null;
    }

    public async Task<int> GetPropertyIdByPublicId( Guid propertyPublicId )
    {
        Property? property = await _dbContext.Properties.FirstOrDefaultAsync( p => p.PublicId == propertyPublicId );
        if ( property is null )
        {
            throw new InvalidOperationException( $"Not found property with id: {propertyPublicId}" );
        }
        return property.Id;
    }

    public async Task<int> GetRoomTypeIdByPublicId( Guid roomTypePublicId )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.PublicId == roomTypePublicId );
        if ( roomType is null )
        {
            throw new InvalidOperationException( $"Not found room type with id: {roomTypePublicId}" );
        }
        return roomType.Id;
    }

    public async Task<string> GetRoomTypeCurrency( int roomTypeId )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == roomTypeId );
        if ( roomType is null )
        {
            throw new InvalidOperationException( $"Not found room type with id: {roomTypeId}" );
        }
        return roomType.Currency;
    }

    public async Task<decimal> GetRoomTypeDailyPrice( int roomTypeId )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == roomTypeId );
        if ( roomType is null )
        {
            throw new InvalidOperationException( $"Not found room type with id: {roomTypeId}" );
        }
        return roomType.DailyPrice;
    }
}
