using AutoMapper;
using Moq;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;

namespace TaskManagerTest;

public class TaskServiceTests
{
    private readonly Mock<IAppUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockUnitOfWork = new Mock<IAppUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new TaskService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldReturnSuccess()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "Test Task", Description = "Description" };
        var taskEntity = new TaskItem();
        var taskDto = new TaskDto();

        // Створюємо мок саме того інтерфейсу, який використовує UnitOfWork.Tasks
        var mockTaskRepository = new Mock<ITaskRepository>();

        // Налаштовуємо UnitOfWork.Tasks -> наш мок
        _mockUnitOfWork.Setup(u => u.Tasks).Returns(mockTaskRepository.Object);

        // Мапер
        _mockMapper.Setup(m => m.Map<TaskItem>(createDto)).Returns(taskEntity);
        _mockMapper.Setup(m => m.Map<TaskDto>(taskEntity)).Returns(taskDto);

        // SaveChangesAsync
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.CreateTaskAsync(createDto);

        // Assert
        Assert.True(result.IsSucces);
        Assert.Equal(taskDto, result.Data);

        // Перевіряємо виклик Insert саме на mockTaskRepository
        mockTaskRepository.Verify(r => r.Insert(taskEntity), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldReturnSuccess_WhenTransitionIsAllowed()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Status = WorkStatus.Backlog };
        var dto = new ChangeStatusDto { NewStatus = "InWork" };

        // Мок для FindAsync без CancellationToken
        _mockUnitOfWork
            .Setup(u => u.Tasks.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Мапер
        _mockMapper.Setup(m => m.Map<WorkStatus>(dto)).Returns(WorkStatus.InWork);

        // SaveChangesAsync
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.ChangeStatusAsync(1, dto);

        // Assert
        Assert.True(result.IsSucces);
        Assert.True(result.Data);
        Assert.Equal(WorkStatus.InWork, task.Status);

        // Verify
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldReturnError_WhenTransitionIsNotAllowed()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Status = WorkStatus.Backlog };
        var dto = new ChangeStatusDto { NewStatus = "Done" };

        _mockUnitOfWork
            .Setup(u => u.Tasks.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        _mockMapper.Setup(m => m.Map<WorkStatus>(dto)).Returns(WorkStatus.Done);

        // Act
        var result = await _service.ChangeStatusAsync(1, dto);

        // Assert
        Assert.False(result.IsSucces);
        Assert.Contains("Неможливий перехід", result.problemDetails.Title);
    }
}