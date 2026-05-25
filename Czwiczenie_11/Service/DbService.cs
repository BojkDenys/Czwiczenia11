using Czwiczenie_11.Dtos;
using Czwiczenie_11.Models;
using Microsoft.EntityFrameworkCore;

namespace Czwiczenie_11.Service;

public class DbService : IDbService
{
    private readonly AppDbContext _context;

    public DbService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<GetPatientDto>> GetPatientsAsync(string? search)
    {
        var query = _context.Patients
            .Include(x => x.Admissions)
            .ThenInclude(x => x.Ward)
            .Include(x => x.BedAssignments)
            .ThenInclude(x => x.Bed)
            .ThenInclude(x => x.BedType)
            .Include(x => x.BedAssignments)
            .ThenInclude(x => x.Bed)
            .ThenInclude(x => x.Room)
            .ThenInclude(x => x.Ward)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.Like(x.FirstName, $"%{search}%") ||
                EF.Functions.Like(x.LastName, $"%{search}%"));
        }

        var patients = await query.ToListAsync();
        return patients.Select(x => new GetPatientDto
        {
            Pesel = x.Pesel,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Age = x.Age,
            Sex = x.Sex? "Male" : "Female",
            Admissions = x.Admissions.Select(a => new GetAdmissionsDto
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new GetWardDto
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            BedAssignments = x.BedAssignments.Select(b => new GetBedAssignmentsDto
            {
                Id = b.Id,
                From = b.From,
                To = b.To,
                Bed = new GetBedDto
                {
                    Id = b.Bed.Id,
                    BedType = new GetBedTypeDto
                    {
                        Id = b.Bed.BedType.Id,
                        Name = b.Bed.BedType.Name,
                        Description = b.Bed.BedType.Description
                    },
                    Room = new GetRoomDto
                    {
                        Id = b.Bed.Room.Id,
                        HasTv = b.Bed.Room.HasTv,
                        Ward = new GetWardDto
                        {
                            Id = b.Bed.Room.Ward.Id,
                            Name = b.Bed.Room.Ward.Name,
                            Description = b.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()

        }).ToList();
    }

    public async Task CreateBedAssignmentAsync(string pesel, CreateBedAssignmentDto createBedAssignment)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(x => x.Pesel == pesel);
        if (patient == null)
        {
            throw new Exception("Patient not found");
        }

        var beds = await _context.Beds
            .Include(x => x.BedType)
            .Include(x => x.Room)
            .ThenInclude(x => x.Ward)
            .ToListAsync();
        var matchingBeds = beds.Where(b =>
            b.BedType.Name == createBedAssignment.BedType &&
            b.Room.Ward.Name == createBedAssignment.Ward);
        Bed? freeBed = null;
        foreach (var bed in matchingBeds)
        {
            var notFree = await _context.BedAssignments.AnyAsync(x =>
                x.BedId == bed.Id &&
                createBedAssignment.From < x.To &&
                (createBedAssignment.To ?? DateTime.MaxValue) > x.From);
            if (!notFree)
            {
                freeBed = bed;
                break;
            }
        }

        if (freeBed == null)
        {
            throw new Exception("Not found free bed");
        }

        var assigment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = freeBed.Id,
            From = createBedAssignment.From,
            To = createBedAssignment.To
        };
        _context.BedAssignments.Add(assigment);
        await _context.SaveChangesAsync();
    }
}