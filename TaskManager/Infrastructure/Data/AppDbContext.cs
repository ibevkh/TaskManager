using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>().ToTable("Tasks");

        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem { Id = 1, Title = "Initial Task 1", Description = "Description 1", Status = WorkStatus.Backlog },
            new TaskItem { Id = 2, Title = "Initial Task 2", Description = "Description 2", Status = WorkStatus.InWork },
            new TaskItem { Id = 3, Title = "Initial Task 3", Description = "Description 3", Status = WorkStatus.Testing }
        );
    }
}
