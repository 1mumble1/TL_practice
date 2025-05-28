using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Database.EntityConfigurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure( EntityTypeBuilder<Property> builder )
    {
        builder.ToTable( nameof( Property ) )
            .HasKey( p => p.Id );

        builder.Property( p => p.Name )
            .HasMaxLength( 100 )
            .IsRequired();

        builder.Property( p => p.Country )
            .HasMaxLength( 50 )
            .IsRequired();

        builder.Property( p => p.City )
            .HasMaxLength( 50 )
            .IsRequired();

        builder.Property( p => p.Address )
            .HasMaxLength( 200 )
            .IsRequired();

        builder.Property( p => p.Latitude )
            .HasPrecision( 18, 10 )
            .IsRequired();

        builder.Property( p => p.Longitude )
            .HasPrecision( 18, 10 )
            .IsRequired();

        builder.HasMany( p => p.RoomTypes )
            .WithOne( rt => rt.Property )
            .HasForeignKey( rt => rt.PropertyId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasMany( p => p.Reservations )
            .WithOne( r => r.Property )
            .HasForeignKey( r => r.PropertyId )
            .OnDelete( DeleteBehavior.Restrict );
    }
}
