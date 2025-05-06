using FluentValidation;

namespace Application.DTOs;

public class TodoItemDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class TodoItemDTOValidator : AbstractValidator<TodoItemDTO>
{
    public TodoItemDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");
        
        RuleFor(x => x.IsCompleted)
            .NotNull()
            .WithMessage("IsCompleted is required.");
    }
}
