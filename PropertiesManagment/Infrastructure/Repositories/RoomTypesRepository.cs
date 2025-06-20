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

    public async Task<IReadOnlyList<RoomType>> GetAllByPropertyId( int propertyId )
    {
        return await _dbContext.RoomTypes
            .AsNoTracking()
            .Include( rt => rt.Property )
            .Where( rt => rt.PropertyId == propertyId )
            .ToListAsync();
    }

    public async Task<RoomType?> GetById( Guid id )
    {
        return await _dbContext.RoomTypes
            .Include( rt => rt.Property )
            .FirstOrDefaultAsync( rt => rt.PublicId == id );
    }

    public async Task<Guid> Create( RoomType roomType )
    {
        await _dbContext.AddAsync( roomType );
        await _dbContext.SaveChangesAsync();
        return roomType.PublicId;
    }

    public async Task Update( RoomType roomType )
    {
        _dbContext.RoomTypes.Update( roomType );
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete( Guid id )
    {
        await _dbContext.RoomTypes
            .Where( rt => rt.PublicId == id )
            .ExecuteDeleteAsync();
    }

    public async Task<Property?> GetPropertyById( Guid propertyId )
    {
        return await _dbContext.Properties.FirstOrDefaultAsync( p => p.PublicId == propertyId );
    }
}