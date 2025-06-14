namespace Application.Contracts;

public record PropertyWithRoomTypesDto
{
    public PropertyDto Property { get; set; }
    public IReadOnlyList<RoomTypeDto> RoomTypes { get; set; }

    public PropertyWithRoomTypesDto( PropertyDto property, IReadOnlyList<RoomTypeDto> roomTypes )
    {
        Property = property;
        RoomTypes = roomTypes;
    }
}
