using SmartSocietyRoutingDemo.Models;

namespace SmartSocietyRoutingDemo.ViewModels;

public class RoutingHomeViewModel
{
    public List<Complaint> Complaints { get; set; } = [];
    public string? SelectedStatus { get; set; }
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public string UrlFromUrlAction { get; set; } = string.Empty;
    public string UrlFromNamedRoute { get; set; } = string.Empty;
}
