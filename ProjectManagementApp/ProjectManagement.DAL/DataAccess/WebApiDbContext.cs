using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.Entities;

namespace ProjectManagement.DAL.DataAccess
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured) 
            {
                options.UseNpgsql("Server=localhost;Database=ProjectApi;Port=5430;User Id=postgres;Password=1123581321;Trust Server Certificate=true;");
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject",
                    j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasKey("EmployeeId", "ProjectId")
                );

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany()
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

