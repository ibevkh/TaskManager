using AutoMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManager.Core.Services;

public class TaskService : ITaskService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly Dictionary<WorkStatus, WorkStatus> _allowedTransitions = new()
        {
            { WorkStatus.Backlog, WorkStatus.InWork },
            { WorkStatus.InWork, WorkStatus.Testing },
            { WorkStatus.Testing, WorkStatus.Done }
        };

    public TaskService(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponseDto<bool>> ChangeStatusAsync(int id, ChangeStatusDto dto)
    {
        var task = await _unitOfWork.Tasks.FindAsync(id);
        if (task == null)
        {
            return new BaseResponseDto<bool>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Завдання не знайдено",
                    Status = 404
                }
            };
        }

        WorkStatus newStatus;
        try
        {
            newStatus = _mapper.Map<WorkStatus>(dto);
        }
        catch (Exception)
        {
            return new BaseResponseDto<bool>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = $"Недопустимий статус: {dto.NewStatus}",
                    Status = 400
                }
            };
        }

        if (!_allowedTransitions.TryGetValue(task.Status, out var allowedNextStatus) || allowedNextStatus != newStatus)
        {
            return new BaseResponseDto<bool>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = $"Неможливий перехід зі статусу {task.Status} в {newStatus}",
                    Status = 400
                }
            };
        }

        task.Status = newStatus;
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponseDto<bool>
        {
            IsSucces = true,
            Data = true
        };
    }

    public async Task<BaseResponseDto<TaskDto>> CreateTaskAsync(CreateTaskDto dto)
    {
        var validator = new CreateTaskDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new BaseResponseDto<TaskDto>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Validation failed: " + errors,
                    Status = 400
                }
            };
        }

        var task = _mapper.Map<TaskItem>(dto);
        task.Status = WorkStatus.Backlog;

        _unitOfWork.Tasks.Insert(task);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponseDto<TaskDto>
        {
            IsSucces = true,
            Data = _mapper.Map<TaskDto>(task)
        };
    }

    public async Task<BaseResponseDto<bool>> DeleteTaskAsync(int id)
    {
        var task = await _unitOfWork.Tasks.FindAsync(id);
        if (task == null)
        {
            return new BaseResponseDto<bool>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Завдання не знайдено",
                    Status = 404
                }
            };
        }

        _unitOfWork.Tasks.Delete(task);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponseDto<bool>
        {
            IsSucces = true,
            Data = true
        };
    }

    public async Task<BaseResponseDto<IEnumerable<TaskDto>>> GetAllTasksAsync()
    {
        var tasks = await _unitOfWork.Tasks.GetAllAsync();
        return new BaseResponseDto<IEnumerable<TaskDto>>
        {
            IsSucces = true,
            Data = _mapper.Map<List<TaskDto>>(tasks.ToList())
        };
    }

    public async Task<BaseResponseDto<TaskDto>> GetTaskByIdAsync(int id)
    {
        var task = await _unitOfWork.Tasks.FindAsync(id);
        if (task == null)
        {
            return new BaseResponseDto<TaskDto>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Завдання не знайдено",
                    Status = 404
                }
            };
        }
        return new BaseResponseDto<TaskDto>
        {
            IsSucces = true,
            Data = _mapper.Map<TaskDto>(task)
        };
    }

    public async Task<BaseResponseDto<TaskDto>> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var validator = new UpdateTaskDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new BaseResponseDto<TaskDto>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Validation failed: " + errors,
                    Status = 400
                }
            };
        }

        var task = await _unitOfWork.Tasks.FindAsync(id);
        if (task == null)
        {
            return new BaseResponseDto<TaskDto>
            {
                IsSucces = false,
                problemDetails = new ProblemDetailsDto
                {
                    Title = "Task not found",
                    Status = 404
                }
            };
        }

        _mapper.Map(dto, task);

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponseDto<TaskDto>
        {
            IsSucces = true,
            Data = _mapper.Map<TaskDto>(task)
        };
    }
}
