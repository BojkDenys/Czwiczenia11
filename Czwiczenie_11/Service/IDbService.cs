using Czwiczenie_11.Dtos;

namespace Czwiczenie_11.Service;

public interface IDbService
{
    Task<List<GetPatientDto>> GetPatientsAsync(string? search);
    Task CreateBedAssignmentAsync(string pesel, CreateBedAssignmentDto createBedAssignment);
}