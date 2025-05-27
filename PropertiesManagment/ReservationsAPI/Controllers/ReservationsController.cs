using Domain.Abstractions.Services;
using Domain.Entities;
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
        List<Domain.Abstractions.Contracts.PropertyWithRoomTypesDto> result = await _reservationsService.SearchAvailableReservations(
            city,
            arrivalDate,
            departureDate,
            guests,
            maxDailyPrice );
        return Ok( result );
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
        List<Reservation> reservations = await _reservationsService.GetAllReservations(
            propertyId,
            roomTypeId,
            arrivalDate,
            departureDate,
            guestName,
            guestPhoneNumber );

        List<ReservationResponse> reservationsResponse = reservations
            .Select( r => new ReservationResponse(
                r.Id,
                r.PropertyId,
                r.RoomTypeId,
                r.ArrivalDate,
                r.DepartureDate,
                r.ArrivalTime,
                r.DepartureTime,
                r.GuestName,
                r.GuestPhoneNumber,
                r.Total,
                r.Currency ) )
            .ToList();

        return Ok( reservationsResponse );
    }

    [HttpGet( "reservations/{id:guid}" )]
    public async Task<IActionResult> GetReservationById( [FromRoute] Guid id )
    {
        var reservation = await _reservationsService.GetReservationById( id );
        if ( reservation is null )
        {
            return NotFound();
        }

        ReservationResponse reservationResponse = new(
            reservation.Id,
            reservation.PropertyId,
            reservation.RoomTypeId,
            reservation.ArrivalDate,
            reservation.DepartureDate,
            reservation.ArrivalTime,
            reservation.DepartureTime,
            reservation.GuestName,
            reservation.GuestPhoneNumber,
            reservation.Total,
            reservation.Currency );

        return Ok( reservationResponse );
    }

    [HttpDelete( "reservations/{id:guid}" )]
    public async Task<IActionResult> DeleteReservationById( [FromRoute] Guid id )
    {
        var result = await _reservationsService.DeleteReservation( id );
        return Ok( result );
    }
}
