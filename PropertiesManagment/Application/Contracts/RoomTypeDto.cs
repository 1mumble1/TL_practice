namespace Application.Contracts;

public record RoomTypeDto(
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