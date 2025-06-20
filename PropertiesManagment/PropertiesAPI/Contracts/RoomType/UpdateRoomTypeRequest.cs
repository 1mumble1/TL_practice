namespace PropertiesAPI.Contracts.RoomType;

public record UpdateRoomTypeRequest(
    Guid PropertyId,
    string Name,
    decimal DailyPrice,
    string Currency,
    int MinPersonCount,
    int MaxPersonCount,
    string Services,
    string Amenities,
    int AvailableRooms );