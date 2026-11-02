namespace SmartSociety.Web.ViewModels;

public class ComplaintIndexViewModel
{
    public string PageTitle { get; set; } = "Complaint List";

    public int TotalComplaints { get; set; }

    public int OpenComplaints { get; set; }

    public int ResolvedComplaints { get; set; }

    public string? SelectedStatus { get; set; }

    public List<ComplaintSummaryViewModel> Complaints { get; set; } = [];
}
