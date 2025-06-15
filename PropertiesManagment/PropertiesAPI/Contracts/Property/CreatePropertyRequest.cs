namespace PropertiesAPI.Contracts.Property;

public record CreatePropertyRequest(
    string Name,
    string Country,
    string City,
    string Address,
    decimal Latitude,
    decimal Longitude );