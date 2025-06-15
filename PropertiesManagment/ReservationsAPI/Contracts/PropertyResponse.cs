namespace ReservationsAPI.Contracts;

public record PropertyResponse(
    Guid Id,
    string Name,
    string Country,
    string City,
    string Address,
    decimal Latitude,
    decimal Longitude );