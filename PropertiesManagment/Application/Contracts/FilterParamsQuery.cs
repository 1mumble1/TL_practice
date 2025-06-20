namespace Application.Contracts;

public record FilterParamsQuery(
    Guid? PropertyId,
    Guid? RoomTypeId,
    DateOnly? ArrivalDate,
    DateOnly? DepartureDate,
    string? GuestName,
    string? GuestPhoneNumber );
