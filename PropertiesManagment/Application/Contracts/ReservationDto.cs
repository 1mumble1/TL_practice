namespace Application.Contracts;

public record ReservationDto(
    Guid Id,
    Guid PropertyId,
    Guid RoomTypeId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    TimeOnly ArrivalTime,
    TimeOnly DepartureTime,
    string GuestName,
    string GuestPhoneNumber,
    decimal Total,
    string Currency );