namespace Advance
{
    public class Complaint
    {
        public string Id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string proritityScore { get; set; }
        public DateTime? resolvedDate { get; set; }=DateTime.Now;
    }
}

