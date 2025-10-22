using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public WorkStatus Status { get; set; }
}
