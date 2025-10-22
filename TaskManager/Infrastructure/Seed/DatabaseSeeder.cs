using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Tasks.Any())
        {
            context.Tasks.AddRange(
                new TaskItem { Title = "Підготувати звіт", Description = "Підсумки тижня", Status = WorkStatus.Backlog },
                new TaskItem { Title = "Налаштувати сервер", Description = "Встановити оновлення", Status = WorkStatus.InWork }
            );

            context.SaveChanges();
        }
    }
}
