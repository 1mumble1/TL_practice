namespace Infrastructure.Repositories;

public class ReservationsRepository
{
    private readonly PropertiesDbContext _dbContext;

    public ReservationsRepository( PropertiesDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    
}
