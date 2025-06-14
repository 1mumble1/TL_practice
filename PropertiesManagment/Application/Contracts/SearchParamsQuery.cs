namespace Application.Contracts;

public record SearchParamsQuery
{
    public string? City { get; set; }
    public DateOnly? ArrivalDate { get; set; }
    public DateOnly? DepartureDate { get; set; }
    public int? Guests { get; set; }
    public decimal? MaxDailyPrice { get; set; }

    public SearchParamsQuery(
        string? city,
        DateOnly? arrivalDate,
        DateOnly? departureDate,
        int? guests,
        decimal? maxDailyPrice )
    {
        City = city;
        ArrivalDate = arrivalDate;
        DepartureDate = departureDate;
        Guests = guests;
        MaxDailyPrice = maxDailyPrice;
    }

    public void Deconstruct(
        out string? city,
        out DateOnly? arrivalDate,
        out DateOnly? departureDate,
        out int? guests,
        out decimal? maxDailyPrice )
    {
        city = City;
        arrivalDate = ArrivalDate;
        departureDate = DepartureDate;
        guests = Guests;
        maxDailyPrice = MaxDailyPrice;
    }
}
