using Application.DTOs;
using MediatR;

namespace Application.Features.TodoItem.Commands;

public class CreateTodoItemCommand : IRequest<int>
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }

    public CreateTodoItemCommand(TodoItemDTO todoItemDTO)
    {
        Title = todoItemDTO.Title!;
        IsCompleted = todoItemDTO.IsCompleted;
    }
}



public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken); // Simulate async work
    
        return 1;
    }
}