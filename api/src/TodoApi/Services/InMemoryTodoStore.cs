using TodoApi.Models;
namespace TodoApi.Services;

public class InMemoryTodoStore: ITodoStore
{
    private readonly List<TodoItem> _items = [];
    private readonly Lock _lock = new();

    public TodoItem Add(string title)
    {
        var item = new TodoItem(Guid.NewGuid(), title.Trim());
        lock(_lock)
        {
            _items.Add(item);
        }
        
        return item;
    }

    public IReadOnlyList<TodoItem> GetAll()
    {
        lock(_items)
        {
            return _items.ToList();
        }
    }

    public bool Remove(Guid Id)
    {
        lock(_lock)
        {
            return _items.RemoveAll(i => i.Id == Id) > 0;
        }
    }
}