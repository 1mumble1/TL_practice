using Application.Abstractions;
using Application.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PropertiesAPI.Contracts.RoomType;

namespace PropertiesAPI.Controllers;

[ApiController]
[Route( "api" )]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypesService _roomTypesService;

    public RoomTypesController( IRoomTypesService roomTypesService )
    {
        _roomTypesService = roomTypesService;
    }

    [HttpGet( "properties/{propertyId:guid}/roomtypes" )]
    public async Task<IActionResult> GetAllRoomTypesByPropertyId( [FromRoute] Guid propertyId )
    {
        try
        {
            IReadOnlyList<RoomTypeDto> roomTypes = await _roomTypesService.GetAllRoomTypesByPropertyId( propertyId );

            IReadOnlyList<RoomTypeResponse> roomTypesResponse = roomTypes
                .Select( rt => new RoomTypeResponse(
                    rt.Id,
                    rt.PropertyId,
                    rt.Name,
                    rt.DailyPrice,
                    rt.Currency,
                    rt.MinPersonCount,
                    rt.MaxPersonCount,
                    rt.Services,
                    rt.Amenities,
                    rt.AvailableRooms ) )
                .ToList();

            return Ok( roomTypesResponse );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpGet( "roomtypes/{id:guid}" )]
    public async Task<IActionResult> GetRoomTypeById( [FromRoute] Guid id )
    {
        RoomTypeDto? roomType = await _roomTypesService.GetRoomTypeById( id );

        if ( roomType is null )
        {
            return NotFound();
        }

        RoomTypeResponse roomTypeResonse = new(
            roomType.Id,
            roomType.PropertyId,
            roomType.Name,
            roomType.DailyPrice,
            roomType.Currency,
            roomType.MinPersonCount,
            roomType.MaxPersonCount,
            roomType.Services,
            roomType.Amenities,
            roomType.AvailableRooms );

        return Ok( roomTypeResonse );
    }

    [HttpPost( "properties/{propertyId:guid}/roomtype" )]
    public async Task<IActionResult> CreateRoomTypeByPropertyId( [FromRoute] Guid propertyId, [FromBody] CreateRoomTypeRequest createRoomTypeRequest )
    {
        try
        {
            Guid result = await _roomTypesService.CreateRoomType(
                propertyId,
                createRoomTypeRequest.Name,
                createRoomTypeRequest.DailyPrice,
                createRoomTypeRequest.Currency,
                createRoomTypeRequest.MinPersonCount,
                createRoomTypeRequest.MaxPersonCount,
                createRoomTypeRequest.Services,
                createRoomTypeRequest.Amenities,
                createRoomTypeRequest.AvailableRooms );
            return Ok( result );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpPut( "roomtypes/{id:guid}" )]
    public async Task<IActionResult> UpdateRoomTypeById( [FromRoute] Guid id, [FromBody] UpdateRoomTypeRequest updateRoomTypeRequest )
    {
        try
        {
            await _roomTypesService.UpdateRoomType(
                id,
                updateRoomTypeRequest.PropertyId,
                updateRoomTypeRequest.Name,
                updateRoomTypeRequest.DailyPrice,
                updateRoomTypeRequest.Currency,
                updateRoomTypeRequest.MinPersonCount,
                updateRoomTypeRequest.MaxPersonCount,
                updateRoomTypeRequest.Services,
                updateRoomTypeRequest.Amenities,
                updateRoomTypeRequest.AvailableRooms );

            return Ok();
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpDelete( "roomtypes/{id:guid}" )]
    public async Task<IActionResult> DeleteRoomTypeById( [FromRoute] Guid id )
    {
        await _roomTypesService.DeleteRoomType( id );
        return Ok();
    }
}
