using Microsoft.AspNetCore.Mvc;

namespace ReservationsAPI.Controllers;

public class ReservationsController : ControllerBase
{
    private readonly ReservationsService _reservationsService;
    public ReservationsController()
    {
    }
}
