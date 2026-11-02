namespace SmartSocietyRoutingDemo.Models;

public class Complaint
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
}
