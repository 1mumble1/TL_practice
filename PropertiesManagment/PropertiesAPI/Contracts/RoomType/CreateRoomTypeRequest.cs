namespace PropertiesAPI.Contracts.RoomType;

public record CreateRoomTypeRequest(
    string Name,
    decimal DailyPrice,
    string Currency,
    int MinPersonCount,
    int MaxPersonCount,
    string Services,
    string Amenities,
    int AvailableRooms );
