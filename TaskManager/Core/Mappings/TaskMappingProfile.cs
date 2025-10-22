using AutoMapper;
using TaskManager.Core.DTOs.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        // TaskItem -> TaskDto
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // CreateTaskDto -> TaskItem
        CreateMap<CreateTaskDto, TaskItem>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => WorkStatus.Backlog));

        // UpdateTaskDto -> TaskItem
        CreateMap<UpdateTaskDto, TaskItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
