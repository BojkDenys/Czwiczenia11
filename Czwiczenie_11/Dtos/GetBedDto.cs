namespace Czwiczenie_11.Dtos;

public class GetBedDto
{
    public int Id { get; set; }
    public GetBedTypeDto BedType { get; set; } = null!;
    public GetRoomDto Room { get; set; } = null!;
}