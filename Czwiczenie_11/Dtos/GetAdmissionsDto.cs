namespace Czwiczenie_11.Dtos;

public class GetAdmissionsDto
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public GetWardDto Ward { get; set; } = null!;
}