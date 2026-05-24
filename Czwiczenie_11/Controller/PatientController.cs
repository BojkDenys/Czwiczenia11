using Czwiczenie_11.Service;
using Microsoft.AspNetCore.Mvc;

namespace Czwiczenie_11.Controller;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly IDbService _service;

    public PatientController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search)
    {
        var result = await _service.GetPatientsAsync(search);
        return Ok(result);
    }
    
}