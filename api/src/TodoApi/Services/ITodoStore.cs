using TodoApi.Models;
namespace TodoApi.Services;

public interface ITodoStore
{
    Task<IReadOnlyList<TodoItem>> GetAll(bool? completed, CancellationToken ct);
    Task<TodoItem?> Add(string title, CancellationToken ct); // null when title already exists
    Task<bool> Remove(Guid Id, CancellationToken ct);

    Task<TodoItem?> Get(Guid Id, CancellationToken ct);

    Task<TodoItem?> Update(Guid Id, string title, bool IsCompleted, CancellationToken ct);
}