using Domain.Entities;

namespace Domain.Abstractions.Contracts;

public class PropertyWithRoomTypesDto
{
    public PropertyDto Property { get; set; }
    public List<RoomTypeDto> RoomTypes { get; set; }
}
