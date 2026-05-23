namespace Czwiczenie_11.Dtos;

public class GetBedAssignmentsDto
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public GetBedDto Bed { get; set; } = null!;
}