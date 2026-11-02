namespace LiveClassDemos.Models
{
    public class Complaint
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Priority { get; set; } = "High";

    }
}
