namespace PropertiesAPI.Contracts.Property;

public record UpdatePropertyRequest(
    string Name,
    string Country,
    string City,
    string Address,
    decimal Latitude,
    decimal Longitude );