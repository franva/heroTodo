using TodoApi.Models;
namespace TodoApi.Services;

public interface ITodoStore
{
    IReadOnlyList<TodoItem> GetAll();
    TodoItem Add(string title);
    bool Remove(Guid Id);

    TodoItem? Get(Guid Id);

    TodoItem? Update(Guid Id, string title, bool IsCompleted = false);
}