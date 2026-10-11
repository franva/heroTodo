using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController(ITodoStore store) : ControllerBase
    {
        // [HttpGet]
        // public ActionResult<IReadOnlyList<TodoItem>> GetAll()
        // {
        //     return Ok(store.GetAll());
        // }

        [HttpGet]
        public IReadOnlyList<TodoItem> GetAll() => store.GetAll();

        [HttpPost]
        public ActionResult<TodoItem> Create(CreateTodoRequest request)
        {
            var item = store.Add(request.Title);
            return Created($"/api/todos/{item.Id}", item);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id) => store.Remove(id) ? NoContent() : NotFound();
    }
}
