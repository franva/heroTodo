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
        public IReadOnlyList<TodoItem> GetAll() => store.GetAll();

        [HttpGet("{id:guid}")]
        public ActionResult<TodoItem?> Get(Guid id) => store.Get(id) is {} item? item : NotFound();

        [HttpPost]
        public ActionResult<TodoItem> Create(CreateTodoRequest request)
        {
            var item = store.Add(request.Title);
            return Created($"/api/todos/{item.Id}", item);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id) => store.Remove(id) ? NoContent() : NotFound();

        [HttpPut("{id:guid}")]
        public ActionResult<TodoItem> Update(Guid id, UpdateTodoItemRequest request)
        {
            return store.Update(id, request.Title, request.IsCompleted) is {} updatedItem ? updatedItem : NotFound();
        }
    }
}
