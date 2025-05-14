using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomTypesRepository : IRoomTypesRepository
{
    private readonly PropertiesDbContext _dbContext;
    public RoomTypesRepository( PropertiesDbContext dbContext )
    {
        _dbContext = dbContext;
    }
    public async Task<List<RoomType>> GetAllByPropertyId( Guid propertyId )
    {
        List<RoomType> roomTypes = await _dbContext.RoomTypes
            .AsNoTracking()
            .Where( rt => rt.PropertyId == propertyId )
            .ToListAsync();

        return roomTypes;
    }

    public async Task<RoomType?> GetById( Guid id )
    {
        RoomType? roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync( rt => rt.Id == id );
        return roomType;
    }

    public async Task<Guid> Create( RoomType roomType )
    {
        if ( !await IsExistsProperty( roomType.PropertyId ) )
        {
            throw new InvalidOperationException( $"Not found property with id: {roomType.PropertyId}" );
        }
        await _dbContext.AddAsync( roomType );
        await _dbContext.SaveChangesAsync();
        return roomType.Id;
    }

    public async Task<Guid> Update( RoomType roomType )
    {
        if ( !await IsExistsProperty( roomType.PropertyId ) )
        {
            throw new InvalidOperationException( $"Not found property with id: {roomType.PropertyId}" );
        }
        _dbContext.RoomTypes.Update( roomType );
        await _dbContext.SaveChangesAsync();

        return roomType.Id;
    }

    public async Task<Guid> Delete( Guid id )
    {
        await _dbContext.RoomTypes
            .Where( rt => rt.Id == id )
            .ExecuteDeleteAsync();

        return id;
    }

    private async Task<bool> IsExistsProperty( Guid propertyId )
    {
        Property? property = await _dbContext.Properties.FirstOrDefaultAsync( p => p.Id == propertyId );

        return property is not null;
    }
}