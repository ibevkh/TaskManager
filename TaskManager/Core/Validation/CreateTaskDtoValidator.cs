using FluentValidation;
using TaskManager.Core.DTOs.Tasks;

namespace TaskManager.Core.Validation;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(100).WithMessage("Task title must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task description is required")
            .MaximumLength(500).WithMessage("Task description must not exceed 500 characters");
    }
}
