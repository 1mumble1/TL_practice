namespace ReservationsAPI.Contracts;

public class PropertyWithRoomTypesResponse
{
    public PropertyResponse Property { get; set; }
    public IReadOnlyList<RoomTypeResponse> RoomTypes { get; set; }

    public PropertyWithRoomTypesResponse( PropertyResponse property, IReadOnlyList<RoomTypeResponse> roomTypes )
    {
        Property = property;
        RoomTypes = roomTypes;
    }
}
