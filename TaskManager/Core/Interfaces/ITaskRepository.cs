using TaskManager.Core.Entities;
using URF.Core.Abstractions;

namespace TaskManager.Core.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
}