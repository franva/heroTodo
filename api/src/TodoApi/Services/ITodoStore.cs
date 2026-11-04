using TodoApi.Models;
namespace TodoApi.Services;

public interface ITodoStore
{
    IReadOnlyList<TodoItem> GetAll(bool? completed=null);
    TodoItem Add(string title);
    bool Remove(Guid Id);

    TodoItem? Get(Guid Id);

    TodoItem? Update(Guid Id, string title, bool IsCompleted = false);
}