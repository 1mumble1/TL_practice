using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PropertiesRepository : IPropertiesRepository
{
    private readonly PropertiesDbContext _dbContext;

    public PropertiesRepository( PropertiesDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Property>> GetAll()
    {
        return await _dbContext.Properties
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Property?> GetById( Guid id )
    {
        return await _dbContext.Properties
            .FirstOrDefaultAsync( p => p.PublicId == id );
    }

    public async Task<Guid> Create( Property property )
    {
        await _dbContext.AddAsync( property );
        await _dbContext.SaveChangesAsync();
        return property.PublicId;
    }

    public async Task Update( Property property )
    {
        _dbContext.Properties.Update( property );
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete( Guid id )
    {
        await _dbContext.Properties
            .Where( p => p.PublicId == id )
            .ExecuteDeleteAsync();
    }
}
