namespace Domain.Entities;

public class Property
{
    public Guid PublicId { get; }
    public int Id { get; }
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Address { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public List<RoomType> RoomTypes { get; private set; } = [];
    public List<Reservation> Reservations { get; private set; } = [];

    public Property(
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        CheckIfNull( name, nameof( name ) );
        CheckIfNull( country, nameof( country ) );
        CheckIfNull( city, nameof( city ) );
        CheckIfNull( address, nameof( address ) );
        CheckCoordinate( latitude, -90, 90 );
        CheckCoordinate( longitude, -180, 180 );

        PublicId = Guid.NewGuid();
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Update(
        string name,
        string country,
        string city,
        string address,
        decimal latitude,
        decimal longitude )
    {
        CheckIfNull( name, nameof( name ) );
        CheckIfNull( country, nameof( country ) );
        CheckIfNull( city, nameof( city ) );
        CheckIfNull( address, nameof( address ) );
        CheckCoordinate( latitude, -90, 90 );
        CheckCoordinate( longitude, -180, 180 );

        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    private void CheckCoordinate( decimal coordinate, decimal minValue, decimal maxValue )
    {
        if ( coordinate < minValue || coordinate > maxValue )
        {
            throw new ArgumentOutOfRangeException( $"Value of {nameof( coordinate )} must be in interval [{minValue}, {maxValue}]" );
        }
    }

    private void CheckIfNull( string value, string nameOfValue )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentNullException( $"{nameOfValue} cannot be null or empty." );
        }
    }
}
