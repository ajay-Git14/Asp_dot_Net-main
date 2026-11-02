using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Complaint
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public int PriorityScore { get; set; }
    public DateTime? ResolvedDate { get; set; }
}

public interface IComplaintService
{
    Task<List<Complaint>> GetComplaintsAsync();
}

public class ComplaintService22 : IComplaintService
{

    public Info(){
            Console.WriteLine("This is a complaint service for society management.");
    }
    public async Task<List<Complaint>> GetComplaintsAsync()
    {
        await Task.Delay(500);

        return new List<Complaint>
        {
            new Complaint { Id = 101, Title = "Lift not working", Status = "New", PriorityScore = 9 },
            new Complaint { Id = 102, Title = "Water leakage", Status = "Assigned", PriorityScore = 7 },
            new Complaint { Id = 103, Title = "Parking issue", Status = "Resolved", PriorityScore = 4, ResolvedDate = DateTime.Now }
        };
    }
}

public class ComplaintService : IComplaintService
{
    public async Task<List<Complaint>> GetComplaintsAsync()
    {
        await Task.Delay(500);

        return new List<Complaint>
        {
            new Complaint { Id = 101, Title = "Lift not working", Status = "New", PriorityScore = 9 },
            new Complaint { Id = 102, Title = "Water leakage", Status = "Assigned", PriorityScore = 7 },
            new Complaint { Id = 103, Title = "Parking issue", Status = "Resolved", PriorityScore = 4, ResolvedDate = DateTime.Now }
        };
    }
}

public class Program
{
    public static async Task Main()
    {
        IComplaintService complaintService = new ComplaintService();

        try
        {
            List<Complaint> complaints = await complaintService.GetComplaintsAsync();

            var criticalComplaints = complaints
                .Where(c => c.PriorityScore >= 8)
                .OrderByDescending(c => c.PriorityScore)
                .ToList();

            Console.WriteLine("Critical Complaints:");

            foreach (Complaint complaint in criticalComplaints)
            {
                string resolvedStatus = complaint.ResolvedDate.HasValue
                    ? $"Resolved on {complaint.ResolvedDate.Value}"
                    : "Not resolved yet";

                Console.WriteLine($"{complaint.Id} - {complaint.Title} - {resolvedStatus}");
            }

            int pendingCount = complaints.Count(c => c.Status != "Resolved");

            Console.WriteLine($"Pending Complaints: {pendingCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}