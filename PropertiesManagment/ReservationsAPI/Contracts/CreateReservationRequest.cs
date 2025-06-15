namespace ReservationsAPI.Contracts;

public record CreateReservationRequest(
    Guid PropertyId,
    Guid RoomTypeId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    TimeOnly ArrivalTime,
    TimeOnly DepartureTime,
    string GuestName,
    string GuestPhoneNumber );