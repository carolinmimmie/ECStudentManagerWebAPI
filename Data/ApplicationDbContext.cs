using ECStudentManagerWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECStudentManagerWebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Student> Students { get; set; }//kopplar in våran entitiet Student
}