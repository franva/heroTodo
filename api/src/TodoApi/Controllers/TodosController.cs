using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController(ITodoStore store) : ControllerBase
    {

        [HttpGet]
        public async Task<IReadOnlyList<TodoItem>> GetAll([FromQuery] bool? completed, CancellationToken token) => 
            await store.GetAll(completed, token);

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TodoItem?>> Get(Guid id, CancellationToken ct) => 
            await store.Get(id, ct) is {} item? item : NotFound();

        [HttpPost]
        public async Task<ActionResult<TodoItem>> Create(CreateTodoRequest request, CancellationToken ct)
        {
            var item = store.Add(request.Title, ct);
            if(item is null)
            {
                return Conflict(new ProblemDetails {Title = "Duplicated todo", Status = StatusCodes.Status409Conflict});
            }
            return Created($"/api/todos/{item.Id}", item);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct) => 
            await store.Remove(id, ct) ? NoContent() : NotFound();

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TodoItem>> Update(Guid id, UpdateTodoItemRequest request, CancellationToken ct)
        {
            return await store.Update(id, request.Title, request.IsCompleted, ct) is {} updatedItem ? updatedItem : NotFound();
        }
    }
}
