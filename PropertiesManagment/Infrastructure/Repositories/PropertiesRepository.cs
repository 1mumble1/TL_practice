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
    public async Task<List<Property>> GetAll()
    {
        List<Property> properties = await _dbContext.Properties
            .AsNoTracking()
            .ToListAsync();

        return properties;
    }

    public async Task<Property?> GetById( Guid id )
    {
        Property? property = await _dbContext.Properties.FirstOrDefaultAsync( p => p.Id == id );
        return property;
    }

    public async Task<Guid> Create( Property property )
    {
        await _dbContext.AddAsync( property );
        await _dbContext.SaveChangesAsync();
        return property.Id;
    }

    public async Task<Guid> Update( Property property )
    {
        //Property? existingProperty = await GetById( property.Id );

        //if ( existingProperty is null )
        //{
        //    throw new InvalidOperationException( $"Property with id {property.Id} does not exist" );
        //}

        //existingProperty.SetName( property.Name );
        //existingProperty.SetCountry( property.Country );
        //existingProperty.SetCity( property.City );
        //existingProperty.SetAddress( property.Address );
        //existingProperty.SetLatitude( property.Latitude );
        //existingProperty.SetLongitude( property.Longitude );
        _dbContext.Properties.Update( property );
        await _dbContext.SaveChangesAsync();

        return property.Id;
    }

    public async Task<Guid> Delete( Guid id )
    {
        await _dbContext.Properties
            .Where( p => p.Id == id )
            .ExecuteDeleteAsync();

        return id;
    }
}
