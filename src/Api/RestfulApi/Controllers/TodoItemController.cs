using Application.DTOs;
using Application.Features.TodoItem.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemp.Api.RestfulApi.Controllers;

[ApiController]
public class TodoItemController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("api/todoitems")]
    public IActionResult GetTodoItems()
    {
        // Simulate fetching todo items from a database
        var todoItems = new List<string> { "Item 1", "Item 2", "Item 3" };
        return Ok(todoItems);
    }

    [HttpPost]
    [Route("api/todoitems")]
    public async Task<Ok<TodoItemDTO>> CreateTodoItem([FromBody] TodoItemDTO todoItemDTO)
    {
        var result = await sender.Send(new CreateTodoItemCommand(todoItemDTO));
        return TypedResults.Ok(todoItemDTO);
    }

    [HttpPost]
    [Route("api/todoitems/draft")]
    [SkipAsyncValidation]
    public async Task<Ok<TodoItemDTO>> DraftUpdateTodoItem([FromBody] TodoItemDTO todoItemDTO)
    {
        // Simulate updating a todo item in a database
        var result = await sender.Send(todoItemDTO);
        return TypedResults.Ok(todoItemDTO);
    }
}