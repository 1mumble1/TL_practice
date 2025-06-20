namespace Application.Contracts;

public record PropertyDto(
    Guid Id,
    string Name,
    string Country,
    string City,
    string Address,
    decimal Latitude,
    decimal Longitude );