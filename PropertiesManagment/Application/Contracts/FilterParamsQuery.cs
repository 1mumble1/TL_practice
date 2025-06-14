namespace Application.Contracts;

public record FilterParamsQuery
{
    public Guid? PropertyId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public DateOnly? ArrivalDate { get; set; }
    public DateOnly? DepartureDate { get; set; }
    public string? GuestName { get; set; }
    public string? GuestPhoneNumber { get; set; }

    public FilterParamsQuery(
        Guid? propertyId,
        Guid? roomTypeId,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        string? guestName,
        string? guestPhoneNumber )
    {
        PropertyId = propertyId;
        RoomTypeId = roomTypeId;
        ArrivalDate = arrivalDate;
        DepartureDate = departureDate;
        GuestName = guestName;
        GuestPhoneNumber = guestPhoneNumber;
    }

    public void Deconstruct(
        out Guid? propertyId,
        out Guid? roomTypeId,
        out DateOnly? arrivalDate,
        out DateOnly? departureDate,
        out string? guestName,
        out string? guestPhoneNumber )
    {
        propertyId = PropertyId;
        roomTypeId = RoomTypeId;
        arrivalDate = ArrivalDate;
        departureDate = DepartureDate;
        guestName = GuestName;
        guestPhoneNumber = GuestPhoneNumber;
    }
}
