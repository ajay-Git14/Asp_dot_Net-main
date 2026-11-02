namespace SmartSociety.Web.ViewModels;

public static class DemoComplaintData
{
    public static List<ComplaintSummaryViewModel> GetComplaints()
    {
        return
        [
            new ComplaintSummaryViewModel
            {
                Id = 1,
                ReferenceNumber = "CMP-2026-0001",
                Title = "Lift not working",
                Description = "The tower A lift is not working from the morning shift.",
                Category = "Maintenance",
                Priority = "High",
                Status = "Open",
                CreatedBy = "Resident User",
                CreatedOn = DateTime.Today.AddDays(-2)
            },
            new ComplaintSummaryViewModel
            {
                Id = 2,
                ReferenceNumber = "CMP-2026-0002",
                Title = "Water leakage near parking",
                Description = "Water leakage is visible near basement parking pillar B2.",
                Category = "Plumbing",
                Priority = "Medium",
                Status = "Assigned",
                CreatedBy = "Resident User",
                CreatedOn = DateTime.Today.AddDays(-1)
            },
            new ComplaintSummaryViewModel
            {
                Id = 3,
                ReferenceNumber = "CMP-2026-0003",
                Title = "Street light repaired",
                Description = "Street light near the main gate was repaired successfully.",
                Category = "Electrical",
                Priority = "Low",
                Status = "Resolved",
                CreatedBy = "Security Desk",
                CreatedOn = DateTime.Today.AddDays(-5)
            },
            new ComplaintSummaryViewModel
            {
                Id = 4,
                ReferenceNumber = "CMP-2026-0004",
                Title = "Garbage pickup delayed",
                Description = "Garbage pickup has been delayed for block C.",
                Category = "Housekeeping",
                Priority = "Medium",
                Status = "Open",
                CreatedBy = "Resident User",
                CreatedOn = DateTime.Today.AddDays(-3)
            }
        ];
    }
}
