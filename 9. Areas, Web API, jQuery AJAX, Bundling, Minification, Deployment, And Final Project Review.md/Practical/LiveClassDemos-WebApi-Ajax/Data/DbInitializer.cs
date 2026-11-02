using LiveClassDemos.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (await context.Complaints.AnyAsync())
            return;

        context.Complaints.AddRange(
            new Complaint
            {
                Title = "Water leakage",
                Description = "Water is leaking near the parking area.",
                Category = "Water",
                Priority = "High",
                Status = "New",
                CreatedDate = DateTime.Now.AddHours(-4)
            },
            new Complaint
            {
                Title = "Streetlight not working",
                Description = "The streetlight near Block B is not working.",
                Category = "Electricity",
                Priority = "Medium",
                Status = "InProgress",
                CreatedDate = DateTime.Now.AddDays(-1)
            },
            new Complaint
            {
                Title = "Garbage collection delayed",
                Description = "Garbage has not been collected since yesterday.",
                Category = "Cleaning",
                Priority = "High",
                Status = "Resolved",
                CreatedDate = DateTime.Now.AddDays(-2)
            }
        );

        await context.SaveChangesAsync();
    }
}
