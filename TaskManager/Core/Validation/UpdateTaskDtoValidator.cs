using FluentValidation;
using TaskManager.Core.DTOs.Tasks;

namespace TaskManager.Core.Validation;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid task Id");

        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Task title must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Descriprion)
            .NotEmpty().WithMessage("Task description is required")
            .MaximumLength(500).WithMessage("Task description must not exceed 500 characters");
    }
}
