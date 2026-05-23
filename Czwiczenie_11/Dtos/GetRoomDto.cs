namespace Czwiczenie_11.Dtos;

public class GetRoomDto
{
    public int Id { get; set; }
    public bool HasTv { get; set; }
    public GetWardDto Ward { get; set; } = null!;
}