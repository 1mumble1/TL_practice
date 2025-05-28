namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; }
    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }
    public Guid RoomTypeId { get; private set; }
    public RoomType RoomType { get; private set; }
    public DateOnly ArrivalDate { get; private set; }
    public DateOnly DepartureDate { get; private set; }
    public TimeOnly ArrivalTime { get; private set; }
    public TimeOnly DepartureTime { get; private set; }
    public string GuestName { get; private set; }
    public string GuestPhoneNumber { get; private set; }
    public decimal Total { get; }
    public string Currency { get; }

    public Reservation(
        Guid propertyId,
        Guid roomTypeId,
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime,
        string guestName,
        string guestPhoneNumber,
        string currency,
        decimal total )
    {
        CheckDatesValidity( arrivalDate, departureDate, arrivalTime, departureTime );
        CheckIfNull( guestName, nameof( guestName ) );
        CheckIfNull( guestPhoneNumber, nameof( guestPhoneNumber ) );
        CheckIfNull( currency, nameof( currency ) );

        Id = Guid.NewGuid();
        PropertyId = propertyId;
        RoomTypeId = roomTypeId;
        ArrivalDate = arrivalDate;
        DepartureDate = departureDate;
        ArrivalTime = arrivalTime;
        DepartureTime = departureTime;
        GuestName = guestName;
        GuestPhoneNumber = guestPhoneNumber;
        Currency = currency;

        Total = total;
    }

    private void CheckIfNull( string value, string nameOfValue )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentNullException( $"{nameOfValue} cannot be null or empty." );
        }
    }

    private void CheckDatesValidity(
        DateOnly arrivalDate,
        DateOnly departureDate,
        TimeOnly arrivalTime,
        TimeOnly departureTime )
    {
        DateTime arrival = arrivalDate.ToDateTime( arrivalTime );
        DateTime departure = departureDate.ToDateTime( departureTime );

        if ( arrival > departure )
        {
            throw new ArgumentException( $"The Date and Time of arrival cannot be greater than Date and Time of departure." );
        }
    }
}