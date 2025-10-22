using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Tasks;
using TaskManager.Core.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: api/tasks
    [HttpGet]
    [ProducesResponseType<BaseResponseDto<IEnumerable<TaskDto>>>(200)]
    [ProducesResponseType<BaseResponseDto<IEnumerable<TaskDto>>>(400)]
    [ProducesResponseType<BaseResponseDto<IEnumerable<TaskDto>>>(500)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _taskService.GetAllTasksAsync();
        if (!response.IsSucces)
            return BadRequest(response);

        return Ok(response);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(200)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(400)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(500)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _taskService.GetTaskByIdAsync(id);
        if (!response.IsSucces)
            return NotFound(response);

        return Ok(response);
    }

    // POST: api/tasks
    [HttpPost]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(200)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(400)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(500)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var response = await _taskService.CreateTaskAsync(dto);
        if (!response.IsSucces)
            return BadRequest(response);

        return Ok(response);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(200)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(400)]
    [ProducesResponseType<BaseResponseDto<TaskDto>>(500)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var response = await _taskService.UpdateTaskAsync(id, dto);
        if (!response.IsSucces)
            return BadRequest(response);

        return Ok(response);
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType<BaseResponseDto<bool>>(200)]
    [ProducesResponseType<BaseResponseDto<bool>>(400)]
    [ProducesResponseType<BaseResponseDto<bool>>(500)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _taskService.DeleteTaskAsync(id);
        if (!response.IsSucces)
            return NotFound(response);

        return Ok(response);
    }

    // PATCH: api/tasks/{id}/status
    [HttpPatch("{id}/status")]
    [ProducesResponseType<BaseResponseDto<bool>>(200)]
    [ProducesResponseType<BaseResponseDto<bool>>(400)]
    [ProducesResponseType<BaseResponseDto<bool>>(500)]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
    {
        var response = await _taskService.ChangeStatusAsync(id, dto);
        if (!response.IsSucces)
            return BadRequest(response);

        return Ok(response);
    }
}
