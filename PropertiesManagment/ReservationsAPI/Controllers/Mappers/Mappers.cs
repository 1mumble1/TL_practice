using Application.Contracts;
using Domain.Entities;
using ReservationsAPI.Contracts;

namespace ReservationsAPI.Controllers.Mappers;

public static class Mappers
{
    public static IReadOnlyList<PropertyWithRoomTypesResponse> Map( this IReadOnlyList<PropertyWithRoomTypesDto> dtos )
    {
        return dtos
            .Select( r => new PropertyWithRoomTypesResponse
            (
                new PropertyResponse(
                    r.Property.Id,
                    r.Property.Name,
                    r.Property.Country,
                    r.Property.City,
                    r.Property.Address,
                    r.Property.Latitude,
                    r.Property.Longitude ),
                r.RoomTypes
                    .Where( rt => rt.PropertyId == r.Property.Id )
                    .Select( rt => new RoomTypeResponse(
                        rt.Id,
                        rt.PropertyId,
                        rt.Name,
                        rt.DailyPrice,
                        rt.Currency,
                        rt.MinPersonCount,
                        rt.MaxPersonCount,
                        rt.Services,
                        rt.Amenities,
                        rt.AvailableRooms ) )
                    .ToList()
            ) )
            .ToList();
    }

    public static IReadOnlyList<ReservationResponse> Map( this IReadOnlyList<ReservationDto> dtos )
    {
        return dtos
            .Select( Map )
            .ToList();
    }

    public static ReservationResponse Map( this ReservationDto dto )
    {
        return new ReservationResponse(
            dto.Id,
            dto.PropertyId,
            dto.RoomTypeId,
            dto.ArrivalDate,
            dto.DepartureDate,
            dto.ArrivalTime,
            dto.DepartureTime,
            dto.GuestName,
            dto.GuestPhoneNumber,
            dto.Total,
            dto.Currency );
    }
}
