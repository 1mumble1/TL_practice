namespace Application.Contracts;

public class PropertyWithRoomTypesDto
{
    public PropertyDto Property { get; set; }
    public IReadOnlyList<RoomTypeDto> RoomTypes { get; set; }
}
