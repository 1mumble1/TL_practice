namespace Domain.Entities;

public class Property
{
    public Guid Id { get; }
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
            decimal longitude
        )
    {
        CheckIfNull( name );
        CheckIfNull( country );
        CheckIfNull( city );
        CheckIfNull( address );
        CheckCoordinate( latitude, -90, 90 );
        CheckCoordinate( longitude, -180, 180 );

        Id = Guid.NewGuid();
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public Property(
            Guid id,
            string name,
            string country,
            string city,
            string address,
            decimal latitude,
            decimal longitude
        )
    {
        CheckIfNull( name );
        CheckIfNull( country );
        CheckIfNull( city );
        CheckIfNull( address );
        CheckCoordinate( latitude, -90, 90 );
        CheckCoordinate( longitude, -180, 180 );

        Id = id;
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

    private void CheckIfNull( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentNullException( $"{nameof( value )} cannot be null or empty." );
        }
    }

    //public void SetName( string name )
    //{
    //    CheckIfNull( name );
    //    Name = name;
    //}

    //public void SetCountry( string country )
    //{
    //    CheckIfNull( country );
    //    Country = country;
    //}

    //public void SetCity( string city )
    //{
    //    CheckIfNull( city );
    //    City = city;
    //}

    //public void SetAddress( string address )
    //{
    //    CheckIfNull( address );
    //    Address = address;
    //}

    //public void SetLatitude( decimal latitude )
    //{
    //    CheckCoordinate( latitude, -90, 90 );
    //    Latitude = latitude;
    //}

    //public void SetLongitude( decimal longitude )
    //{
    //    CheckCoordinate( longitude, -180, 180 );
    //    Longitude = longitude;
    //}
}
