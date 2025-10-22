using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Repositories;
using URF.Core.EF;

namespace TaskManager.Infrastructure.Data;

public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
{
    private readonly AppDbContext _context;
    private ITaskRepository _tasks;

    public AppUnitOfWork(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
