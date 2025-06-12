using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation.Database.EntityConfigurations;

public class RoomTypeConfiguratuion : IEntityTypeConfiguration<RoomType>
{
    public void Configure( EntityTypeBuilder<RoomType> builder )
    {
        builder.ToTable( nameof( RoomType ) )
            .HasKey( rt => rt.Id );

        builder.Property( rt => rt.PublicId );

        builder.Property( rt => rt.Name )
            .HasMaxLength( 100 )
            .IsRequired();

        builder.Property( rt => rt.DailyPrice )
            .HasPrecision( 18, 2 )
            .IsRequired();

        builder.Property( rt => rt.Currency )
            .HasMaxLength( 25 )
            .IsRequired();

        builder.Property( rt => rt.MinPersonCount )
            .IsRequired();

        builder.Property( rt => rt.MaxPersonCount )
            .IsRequired();

        builder.Property( rt => rt.Services )
            .HasMaxLength( 500 )
            .IsRequired();

        builder.Property( rt => rt.Amenities )
            .HasMaxLength( 500 )
            .IsRequired();

        builder.Property( rt => rt.AvailableRooms )
            .IsRequired();

        builder.HasMany( rt => rt.Reservations )
            .WithOne( r => r.RoomType )
            .HasForeignKey( r => r.RoomTypeId )
            .OnDelete( DeleteBehavior.Restrict );

        builder.HasIndex( rt => rt.PublicId )
            .IsUnique();
    }
}