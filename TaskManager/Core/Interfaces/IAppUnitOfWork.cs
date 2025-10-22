namespace TaskManager.Core.Interfaces;

public interface IAppUnitOfWork
{
    ITaskRepository Tasks { get; }
    Task<int> SaveChangesAsync();
}
