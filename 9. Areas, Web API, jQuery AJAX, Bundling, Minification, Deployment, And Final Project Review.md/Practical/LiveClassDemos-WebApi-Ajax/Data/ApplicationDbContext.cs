using LiveClassDemos.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveClassDemos.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Complaint> Complaints => Set<Complaint>();
}
