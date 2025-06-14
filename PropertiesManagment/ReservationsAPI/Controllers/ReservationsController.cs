using Application.Abstractions;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using ReservationsAPI.Contracts;

namespace ReservationsAPI.Controllers;

[ApiController]
[Route( "api" )]
public class ReservationsController : ControllerBase
{
    private readonly IReservationsService _reservationsService;

    public ReservationsController( IReservationsService reservationsService )
    {
        _reservationsService = reservationsService;
    }

    [HttpGet( "search" )]
    public async Task<IActionResult> SearchAvailableReservations(
        [FromQuery] string? city,
        [FromQuery] DateOnly? arrivalDate,
        [FromQuery] DateOnly? departureDate,
        [FromQuery] int? guests,
        [FromQuery] decimal? maxDailyPrice )
    {
        try
        {
            IReadOnlyList<PropertyWithRoomTypesDto> availableReservations = await _reservationsService.SearchAvailableReservations( new SearchParamsQuery(
                city,
                arrivalDate,
                departureDate,
                guests,
                maxDailyPrice ) );

            IReadOnlyList<PropertyWithRoomTypesResponse> availableReservationsResponse = Mappers.Mappers.Map( availableReservations );

            return Ok( availableReservationsResponse );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpPost( "reservations" )]
    public async Task<IActionResult> CreateReservation( [FromBody] CreateReservationRequest createReservationRequest )
    {
        try
        {
            Guid result = await _reservationsService.CreateReservation(
                createReservationRequest.PropertyId,
                createReservationRequest.RoomTypeId,
                createReservationRequest.ArrivalDate,
                createReservationRequest.DepartureDate,
                createReservationRequest.ArrivalTime,
                createReservationRequest.DepartureTime,
                createReservationRequest.GuestName,
                createReservationRequest.GuestPhoneNumber );

            return Ok( result );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpGet( "reservations" )]
    public async Task<IActionResult> GetAllReservations(
        [FromQuery] Guid? propertyId,
        [FromQuery] Guid? roomTypeId,
        [FromQuery] DateOnly? arrivalDate,
        [FromQuery] DateOnly? departureDate,
        [FromQuery] string? guestName,
        [FromQuery] string? guestPhoneNumber )
    {
        IReadOnlyList<ReservationDto> reservations = await _reservationsService.GetAllReservations( new FilterParamsQuery( propertyId,
            roomTypeId,
            arrivalDate,
            departureDate,
            guestName,
            guestPhoneNumber ) );

        IReadOnlyList<ReservationResponse> reservationsResponse = Mappers.Mappers.Map( reservations );

        return Ok( reservationsResponse );
    }

    [HttpGet( "reservations/{id:guid}" )]
    public async Task<IActionResult> GetReservationById( [FromRoute] Guid id )
    {
        ReservationDto? reservation = await _reservationsService.GetReservationById( id );
        if ( reservation is null )
        {
            return NotFound();
        }

        ReservationResponse reservationResponse = Mappers.Mappers.Map( reservation );

        return Ok( reservationResponse );
    }

    [HttpDelete( "reservations/{id:guid}" )]
    public async Task<IActionResult> DeleteReservationById( [FromRoute] Guid id )
    {
        await _reservationsService.DeleteReservation( id );
        return Ok();
    }
}
