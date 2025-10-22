using AutoMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Mappings;

public class StatusMappingProfile : Profile
{
    public StatusMappingProfile()
    {
        // ChangeStatusDto -> WorkStatus
        CreateMap<ChangeStatusDto, WorkStatus>()
            .ConvertUsing(dto => System.Enum.Parse<WorkStatus>(dto.NewStatus, true));
    }
}
