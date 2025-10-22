using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Tasks;

namespace TaskManager.Core.Interfaces;

public interface ITaskService
{
    Task<BaseResponseDto<TaskDto>> CreateTaskAsync(CreateTaskDto dto);
    Task<BaseResponseDto<IEnumerable<TaskDto>>> GetAllTasksAsync();
    Task<BaseResponseDto<TaskDto>> GetTaskByIdAsync(int id);
    Task<BaseResponseDto<TaskDto>> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<BaseResponseDto<bool>> ChangeStatusAsync(int id, ChangeStatusDto dto);
    Task<BaseResponseDto<bool>> DeleteTaskAsync(int id);
}
