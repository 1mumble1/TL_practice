namespace ReservationsAPI.Contracts;

public class PropertyWithRoomTypesResponse(
    PropertyResponse Property,
    IReadOnlyList<RoomTypeResponse> RoomTypes );