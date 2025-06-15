namespace Application.Contracts;

public record PropertyWithRoomTypesDto(
    PropertyDto Property,
    IReadOnlyList<RoomTypeDto> RoomTypes );