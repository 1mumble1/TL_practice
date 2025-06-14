namespace Domain.Entities;

public class RoomType
{
    public Guid PublicId { get; }
    public int Id { get; }
    public int PropertyId { get; private set; }
    public Property Property { get; private set; }
    public string Name { get; private set; }
    public decimal DailyPrice { get; private set; }
    public string Currency { get; private set; }
    public int MinPersonCount { get; private set; }
    public int MaxPersonCount { get; private set; }
    public string Services { get; private set; }
    public string Amenities { get; private set; }
    public int AvailableRooms { get; private set; }
    public List<Reservation> Reservations { get; private set; } = [];

    public RoomType(
        int propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms )
    {
        CheckIfNull( name, nameof( name ) );
        CheckIfNull( currency, nameof( currency ) );
        CheckIfNull( services, nameof( services ) );
        CheckIfNull( amenities, nameof( amenities ) );
        if ( minPersonCount <= 0 ||
            maxPersonCount <= 0 ||
            minPersonCount > maxPersonCount )
        {
            throw new ArgumentException( $"{nameof( minPersonCount )} and {nameof( maxPersonCount )} must be greater than 0," +
                $"{nameof( minPersonCount )} must be less than {nameof( maxPersonCount )}" );
        }
        if ( availableRooms < 1 )
        {
            throw new ArgumentException( $"{nameof( availableRooms )} must be greater than 1" );
        }
        if ( dailyPrice <= 0 )
        {
            throw new ArgumentException( $"{nameof( dailyPrice )} cannot be less or equal 0" );
        }

        PublicId = Guid.NewGuid();
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

    public void Update(
        int propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        string services,
        string amenities,
        int availableRooms )
    {
        CheckIfNull( name, nameof( name ) );
        CheckIfNull( currency, nameof( currency ) );
        CheckIfNull( services, nameof( services ) );
        CheckIfNull( amenities, nameof( amenities ) );
        if ( minPersonCount <= 0 ||
            maxPersonCount <= 0 ||
            minPersonCount > maxPersonCount )
        {
            throw new ArgumentException( $"{nameof( minPersonCount )} and {nameof( maxPersonCount )} must be greater than 0," +
                $"{nameof( minPersonCount )} must be less than {nameof( maxPersonCount )}" );
        }
        if ( availableRooms < 1 )
        {
            throw new ArgumentException( $"{nameof( availableRooms )} must be greater than 1" );
        }
        if ( dailyPrice <= 0 )
        {
            throw new ArgumentException( $"{nameof( dailyPrice )} cannot be less or equal 0" );
        }

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

    private void CheckIfNull( string value, string nameOfValue )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentNullException( $"{nameOfValue} cannot be null or empty." );
        }
    }
}