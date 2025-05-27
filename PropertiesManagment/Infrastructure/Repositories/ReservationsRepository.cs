using Domain.Abstractions.Contracts;
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

    public async Task<List<PropertyWithRoomTypesDto>> SearchAvailable(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice )
    {
        if ( arrivalDate.HasValue && !departureDate.HasValue ||
            !arrivalDate.HasValue && departureDate.HasValue )
        {
            throw new ArgumentException( "Cannot search with only one date" );
        }

        // property by city
        IQueryable<Property> propertiesQuery = city is null
            ? _dbContext.Properties.AsQueryable()
            : _dbContext.Properties.Where( p => p.City == city );

        // room type by guests count
        IQueryable<RoomType> roomTypesQuery = _dbContext.RoomTypes.AsQueryable();

        if ( guests.HasValue )
        {
            roomTypesQuery = roomTypesQuery.Where( rt =>
                rt.MinPersonCount <= guests && rt.MaxPersonCount >= guests );
        }

        if ( maxDailyPrice.HasValue )
        {
            roomTypesQuery = roomTypesQuery.Where( rt => rt.DailyPrice <= maxDailyPrice );
        }

        // conflicting reservations if we have arrival and departure dates; else empty list
        List<Reservation> conflictingReservations = arrivalDate.HasValue
            ? await _dbContext.Reservations
                .Where( r => r.ArrivalDate < departureDate && r.DepartureDate > arrivalDate )
                .ToListAsync()
            : new List<Reservation>();

        // group
        Dictionary<Guid, int> bookedRoomTypes = conflictingReservations
            .GroupBy( r => r.RoomTypeId )
            .ToDictionary( g => g.Key, g => g.Count() );

        var allRoomTypes = await roomTypesQuery.ToListAsync();

        var availableRoomTypes = allRoomTypes
            .Where( rt =>
            {
                if ( !bookedRoomTypes.TryGetValue( rt.Id, out var bookedCount ) )
                    return true;

                return bookedCount < rt.AvailableRooms;
            } )
            .ToList();

        var propertyIds = availableRoomTypes.Select( rt => rt.PropertyId ).Distinct();
        var availableProperties = await propertiesQuery
            .Where( p => propertyIds.Contains( p.Id ) )
            .ToListAsync();

        //List<Property> result = availableProperties
        //    .Select( p => new Property(
        //        p,
        //        availableRoomTypes.Where( rt => rt.PropertyId == p.Id ).ToList() ) )
        //    .ToList();

        var result = availableProperties.Select( p => new PropertyWithRoomTypesDto
        {
            Property = new PropertyDto(
                p.Id,
                p.Name,
                p.Country,
                p.City,
                p.Address,
                p.Latitude,
                p.Longitude ),
            RoomTypes = availableRoomTypes
                .Where( rt => rt.PropertyId == p.Id )
                .Select( rt => new RoomTypeDto(
                    rt.Id,
                    rt.Name,
                    rt.DailyPrice,
                    rt.Currency,
                    rt.MinPersonCount,
                    rt.MaxPersonCount,
                    rt.Services,
                    rt.Amenities,
                    rt.AvailableRooms ) )
                .ToList()
        } ).ToList();

        return result;
    }

    public async Task<Guid> Create( Reservation reservation )
    {
        await _dbContext.AddAsync( reservation );
        await _dbContext.SaveChangesAsync();
        return reservation.Id;
    }

    public async Task<List<Reservation>> GetAll(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber )
    {
        List<Reservation> result = await _dbContext.Reservations.AsNoTracking().ToListAsync();
        if ( propertyId.HasValue )
        {
            result = result.Where( r => r.PropertyId == propertyId ).ToList();
        }
        if ( roomTypeId.HasValue )
        {
            result = result.Where( r => r.RoomTypeId == roomTypeId ).ToList();
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
        Reservation? reservation = await _dbContext.Reservations.FirstOrDefaultAsync( r => r.Id == id );
        return reservation;
    }

    public async Task<Guid> Delete( Guid id )
    {
        await _dbContext.Reservations
            .Where( r => r.Id == id )
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<bool> ExistsProperty( Guid propertyId )
    {
        return await _dbContext.Properties.FirstOrDefaultAsync( p => p.Id == propertyId ) is not null;
    }

    public async Task<bool> ExistsRoomType( Guid roomTypeId )
    {
        return await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == roomTypeId ) is not null;
    }

    public async Task<string> GetRoomTypeCurrency( Guid roomTypeId )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == roomTypeId );
        if ( roomType is null )
        {
            throw new InvalidOperationException( $"Cannot find room type with id {roomTypeId}" );
        }
        return roomType.Currency;
    }

    public async Task<decimal> GetRoomTypeDailyPrice( Guid roomTypeId )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == roomTypeId );
        if ( roomType is null )
        {
            throw new InvalidOperationException( $"Cannot find room type with id {roomTypeId}" );
        }
        return roomType.DailyPrice;
    }
}
