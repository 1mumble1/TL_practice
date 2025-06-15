namespace Application.Contracts;

public record SearchParamsQuery(
    string? City,
    DateOnly? ArrivalDate,
    DateOnly? DepartureDate,
    int? Guests,
    decimal? MaxDailyPrice );
