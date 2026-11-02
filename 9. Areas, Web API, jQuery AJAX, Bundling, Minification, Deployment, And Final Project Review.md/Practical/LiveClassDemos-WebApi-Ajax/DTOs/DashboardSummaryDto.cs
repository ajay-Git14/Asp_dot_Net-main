namespace LiveClassDemos.DTOs;

public class DashboardSummaryDto
{
    public int TotalComplaints { get; set; }
    public int NewComplaints { get; set; }
    public int InProgressComplaints { get; set; }
    public int ResolvedComplaints { get; set; }
    public int HighPriorityComplaints { get; set; }
}
