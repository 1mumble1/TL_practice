using Domain.Abstractions.Services;
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
        List<RoomType> roomTypes = await _roomTypesService.GetAllRoomTypesByPropertyId( propertyId );

        var roomTypesResponse = roomTypes.Select( rt => new RoomTypeResponse( rt.Id, rt.PropertyId, rt.Name, rt.DailyPrice, rt.Currency, rt.MinPersonCount, rt.MaxPersonCount, rt.Services, rt.Amenties ) ).ToList();

        return Ok( roomTypesResponse );
    }

    [HttpGet( "roomtypes/{id:guid}" )]
    public async Task<IActionResult> GetRoomTypeById( [FromRoute] Guid id )
    {
        RoomType? roomType = await _roomTypesService.GetRoomTypeById( id );

        if ( roomType is null )
        {
            return NotFound();
        }

        RoomTypeResponse roomTypeResonse = new( roomType.Id, roomType.PropertyId, roomType.Name, roomType.DailyPrice, roomType.Currency, roomType.MinPersonCount, roomType.MaxPersonCount, roomType.Services, roomType.Amenties );

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
                    createRoomTypeRequest.Amenties
                );
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
            Guid result = await _roomTypesService.UpdateRoomType(
                    id,
                    updateRoomTypeRequest.PropertyId,
                    updateRoomTypeRequest.Name,
                    updateRoomTypeRequest.DailyPrice,
                    updateRoomTypeRequest.Currency,
                    updateRoomTypeRequest.MinPersonCount,
                    updateRoomTypeRequest.MaxPersonCount,
                    updateRoomTypeRequest.Services,
                    updateRoomTypeRequest.Amenties
                );

            return Ok( result );
        }
        catch ( Exception ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpDelete( "roomtypes/{id:guid}" )]
    public async Task<IActionResult> DeleteRoomTypeById( [FromRoute] Guid id )
    {
        var result = await _roomTypesService.DeleteRoomType( id );
        return Ok( result );
    }
}
