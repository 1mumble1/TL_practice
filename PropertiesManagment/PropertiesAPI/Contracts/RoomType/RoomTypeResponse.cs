namespace PropertiesAPI.Contracts.RoomType;

public record RoomTypeResponse(
    Guid Id,
    Guid PropertyId,
    string Name,
    decimal DailyPrice,
    string Currency,
    int MinPersonCount,
    int MaxPersonCount,
    string Services,
    string Amenities,
    int AvailableRooms );