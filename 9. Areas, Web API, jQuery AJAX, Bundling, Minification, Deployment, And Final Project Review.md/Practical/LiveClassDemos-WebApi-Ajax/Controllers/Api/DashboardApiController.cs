using LiveClassDemos.Data;
using LiveClassDemos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Controllers.Api;

[Route("api/dashboard")]
[ApiController]
public class DashboardApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardApiController(ApplicationDbContext context) => _context = context;

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var complaints = _context.Complaints.AsNoTracking().Where(c => !c.IsArchived);

        var summary = new DashboardSummaryDto
        {
            TotalComplaints = await complaints.CountAsync(),
            NewComplaints = await complaints.CountAsync(c => c.Status == "New"),
            InProgressComplaints = await complaints.CountAsync(c => c.Status == "InProgress"),
            ResolvedComplaints = await complaints.CountAsync(c => c.Status == "Resolved"),
            HighPriorityComplaints = await complaints.CountAsync(c => c.Priority == "High")
        };

        return Ok(summary);
    }
}
