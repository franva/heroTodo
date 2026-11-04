using TodoApi.Models;
namespace TodoApi.Services;

public class InMemoryTodoStore: ITodoStore
{
    private readonly List<TodoItem> _items = [];
    private readonly Lock _lock = new();

    public TodoItem? Add(string title)
    {
        var trimmed = title.Trim();

        lock(_lock)
        {
            if(_items.Any(i => string.Equals(i.Title, trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var item = new TodoItem(Guid.NewGuid(), trimmed);
            _items.Add(item);
            return item;
        }
    }

    public TodoItem? Update(Guid Id, string title, bool IsCompleted)
    {
        lock(_lock)
        {
            var index = _items.FindIndex(i => i.Id == Id);
            if( index < 0)
            {
                return null;
            }
            var updated = _items[index] with {Title = title, IsCompleted = IsCompleted};
            _items[index] = updated;
            return updated;
        }
    }

    public IReadOnlyList<TodoItem> GetAll(bool? completed=null)
    {
        lock(_items)
        {
            return _items.Where(i => completed is null || i.IsCompleted == completed).ToList();
        }
    }

    public bool Remove(Guid Id)
    {
        lock(_lock)
        {
            return _items.RemoveAll(i => i.Id == Id) > 0;
        }
    }

    public TodoItem? Get(Guid Id)
    {
        lock(_items)
        {
            return _items.FirstOrDefault(i => i.Id == Id);
        }
    }
}