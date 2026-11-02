using LiveClassDemos.Data;
using LiveClassDemos.DTOs;
using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Controllers.Api;

[Route("api/complaints")]
[ApiController]
public class ComplaintsApiController : ControllerBase
{
    private static readonly string[] AllowedStatuses = ["New", "InProgress", "Resolved"];
    private readonly ApplicationDbContext _context;

    public ComplaintsApiController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComplaintListDto>>> GetComplaints([FromQuery] string? status)
    {
        var query = _context.Complaints.AsNoTracking().Where(c => !c.IsArchived);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status == status);

        var complaints = await query
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => new ComplaintListDto
            {
                Id = c.Id,
                Title = c.Title,
                Category = c.Category,
                Priority = c.Priority,
                Status = c.Status,
                CreatedDate = c.CreatedDate
            })
            .ToListAsync();

        return Ok(complaints);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintDetailsDto>> GetComplaintById(int id)
    {
        var complaint = await _context.Complaints.AsNoTracking()
            .Where(c => c.Id == id && !c.IsArchived)
            .Select(c => new ComplaintDetailsDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Category = c.Category,
                Priority = c.Priority,
                Status = c.Status,
                CreatedDate = c.CreatedDate
            })
            .FirstOrDefaultAsync();

        return complaint is null
            ? NotFound(new { message = "Complaint not found." })
            : Ok(complaint);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComplaint(CreateComplaintDto dto)
    {
        var complaint = new Complaint
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Category = dto.Category.Trim(),
            Priority = dto.Priority.Trim(),
            Status = "New",
            CreatedDate = DateTime.Now
        };

        _context.Complaints.Add(complaint);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, new
        {
            message = "Complaint created successfully.",
            complaintId = complaint.Id
        });
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
    {
        if (!AllowedStatuses.Contains(dto.NewStatus, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { message = "Status must be New, InProgress, or Resolved." });

        var complaint = await _context.Complaints
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsArchived);

        if (complaint is null)
            return NotFound(new { message = "Complaint not found." });

        complaint.Status = AllowedStatuses.First(s =>
            s.Equals(dto.NewStatus, StringComparison.OrdinalIgnoreCase));

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Complaint status updated successfully.",
            complaintId = complaint.Id,
            newStatus = complaint.Status,
            remarks = dto.Remarks
        });
    }
}
