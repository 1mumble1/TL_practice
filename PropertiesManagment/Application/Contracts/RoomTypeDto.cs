namespace Application.Contracts;

public record RoomTypeDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; }
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public string Services { get; set; }
    public string Amenities { get; set; }
    public int AvailableRooms { get; set; }

    public RoomTypeDto(
        Guid id,
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms )
    {
        Id = id;
        PropertyId = propertyId;
        Name = name;
        DailyPrice = dailyPrice;
        Currency = currency;
        MinPersonCount = minPersonCount;
        MaxPersonCount = maxPersonCount;
        Services = services;
        Amenities = amenities;
        AvailableRooms = availableRooms;
    }
}
