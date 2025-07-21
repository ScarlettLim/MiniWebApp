using Microsoft.EntityFrameworkCore;
using WebApp1.Models;

namespace WebApp1.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<WorkSheet> WorkSheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ProjectTask to Project relationship
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configure WorkSheet relationships
            modelBuilder.Entity<WorkSheet>()
                .HasOne(ws => ws.Project)
                .WithMany()
                .HasForeignKey(ws => ws.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);  

            modelBuilder.Entity<WorkSheet>()
                .HasOne(ws => ws.Task)
                .WithMany()
                .HasForeignKey(ws => ws.ProjectTaskId)
                .OnDelete(DeleteBehavior.NoAction);  

            // Unique constraint
            modelBuilder.Entity<WorkSheet>()
                .HasIndex(ws => new { ws.Date, ws.ProjectId, ws.ProjectTaskId })
                .IsUnique();
        }
    }
}
