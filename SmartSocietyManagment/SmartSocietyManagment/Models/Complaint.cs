namespace SmartSocietyManagment.Models
{
    public class Complaint
    {
        public int ComplaintId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = "New";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}