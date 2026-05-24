namespace Czwiczenie_11.Dtos;

public class GetPatientDto
{
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool Sex { get; set; }
    public List<GetAdmissionsDto> Admissions { get; set; } = [];
    public List<GetBedAssignmentsDto> BedAssignments { get; set; } = [];
}