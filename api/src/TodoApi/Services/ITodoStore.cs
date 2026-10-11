using TodoApi.Models;
namespace TodoApi.Services;

public interface ITodoStore
{
    IReadOnlyList<TodoItem> GetAll();
    TodoItem Add(string title);
    bool Remove(Guid Id);
}