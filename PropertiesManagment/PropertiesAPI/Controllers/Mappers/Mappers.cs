using Application.Contracts;
using Domain.Entities;
using PropertiesAPI.Contracts.Property;
using PropertiesAPI.Contracts.RoomType;

namespace PropertiesAPI.Controllers.Mappers;

public static class Mappers
{
    public static IReadOnlyList<PropertyResponse> Map( this IReadOnlyList<PropertyDto> dtos )
    {
        return dtos
            .Select( Map )
            .ToList();
    }

    public static PropertyResponse Map( this PropertyDto dto )
    {
        return new PropertyResponse(
            dto.Id,
            dto.Name,
            dto.Country,
            dto.City,
            dto.Address,
            dto.Latitude,
            dto.Longitude );
    }

    public static IReadOnlyList<RoomTypeResponse> Map( this IReadOnlyList<RoomTypeDto> dtos )
    {
        return dtos
            .Select( Map )
            .ToList();
    }

    public static RoomTypeResponse Map( this RoomTypeDto dto )
    {
        return new RoomTypeResponse(
            dto.Id,
            dto.PropertyId,
            dto.Name,
            dto.DailyPrice,
            dto.Currency,
            dto.MinPersonCount,
            dto.MaxPersonCount,
            dto.Services,
            dto.Amenities,
            dto.AvailableRooms );
    }
}
