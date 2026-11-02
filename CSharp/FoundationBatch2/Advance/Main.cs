namespace Advance
{
    public class Main
    {   
        public static void Main(string[] args)
        {
            List<Complaint> complaints = new List<Complaint>
            {
                new Complaint { Id = "1", title = "Late delivery", status = "Open", proritityScore = "High" },
                new Complaint { Id = "2", title = "Damaged product", status = "Resolved", proritityScore = "Medium", resolvedDate = DateTime.Now.AddDays(-2) },
                new Complaint { Id = "3", title = "Wrong item received", status = "Open", proritityScore = "Low" }
            };

            var openComplaints = complaints.Where(c => c.status == "Open").ToList();
            Console.WriteLine("Open Complaints:");
            var selectedComplaints = openComplaints.Select(c => new { c.Id, c.title, c.proritityScore });
            foreach (var complaint in selectedComplaints)            {
                Console.WriteLine($"Id: {complaint.Id}, Title: {complaint.title}, Priority: {complaint.proritityScore}");
            }   
            //sort by priority
            var sortedComplaints = openComplaints.OrderBy(c => c.proritityScore).ToList();
            Console.WriteLine("\nSorted Open Complaints by Priority:"); 
            foreach (var complaint in sortedComplaints)
            {
                Console.WriteLine($"Id: {complaint.Id}, Title: {complaint.title}, Priority: {complaint.proritityScore}");
            }

            //Lambda expression to update the status of a complaint
            var sumOfTwoNumbers = (int a, int b) => a + b;
            int result = sumOfTwoNumbers(5, 10);
            Console.WriteLine($"\nSum of two numbers: {result}");
        }
    }
}