namespace Application.Services;

public class ReservationsService
{
    private readonly ReservationsRepository _reservationsRepository;

    public ReservationsService( ReservationsRepository reservationsRepository )
    {
        _reservationsRepository = reservationsRepository;
    }
}
