using Domain.Entities;
using Infrastructure.Foundation.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PropertiesDbContext : DbContext
{
    public PropertiesDbContext( DbContextOptions<PropertiesDbContext> options )
        : base( options )
    { }

    public DbSet<Property> Properties { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfiguration( new PropertyConfiguration() );
        modelBuilder.ApplyConfiguration( new RoomTypeConfiguratuion() );
        modelBuilder.ApplyConfiguration( new ReservationConfiguration() );
    }
}
