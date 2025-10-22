using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Data;
using URF.Core.EF;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await base.Queryable().AsNoTracking().ToListAsync();
    }
}
