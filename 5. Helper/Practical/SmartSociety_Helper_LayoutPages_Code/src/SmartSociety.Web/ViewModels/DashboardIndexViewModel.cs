namespace SmartSociety.Web.ViewModels;

public class DashboardIndexViewModel
{
    public string PageTitle { get; set; } = "Dashboard";

    public string PageSubtitle { get; set; } = string.Empty;

    public int TotalComplaints { get; set; }

    public int OpenComplaints { get; set; }

    public int AssignedComplaints { get; set; }

    public int ResolvedComplaints { get; set; }

    public List<ComplaintSummaryViewModel> RecentComplaints { get; set; } = [];
}
