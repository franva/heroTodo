using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

public class EfTodoStore(TodoDbContext db) : ITodoStore
{
    public async Task<IReadOnlyList<TodoItem>> GetAll(bool? completed, CancellationToken ct) =>
        await db.Todos.AsNoTracking()
            .Where(i => completed == null || i.IsCompleted == completed)
            .OrderBy(i => i.CreatedAt).ToListAsync(ct);

    public async Task<TodoItem?> Add(string title, CancellationToken ct)
    {
        var trimmed = title.Trim();
        var found = await db.Todos.FirstOrDefaultAsync(i => i.Title.ToLower() == trimmed.ToLower(), ct);
        if (found != null)
        {
            return null;
        }
        var item = new TodoItem(Guid.NewGuid(), trimmed, false, DateTime.UtcNow);
        db.Todos.Add(item);
        
        await db.SaveChangesAsync(ct);
        return item;
    }

    public async Task<TodoItem?> Update(Guid id, string title, bool isCompleted, CancellationToken ct)
    {
        var rows = await db.Todos
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(t => t.SetProperty(i => i.Title, title)
                                      .SetProperty(i => i.IsCompleted, isCompleted), ct);

        return rows == 0 ? null : await db.Todos.AsNoTracking().FirstAsync(i => i.Id == id, ct);
    }

    public async Task<bool> Remove(Guid id, CancellationToken ct) =>
        await db.Todos.Where(i => i.Id == id).ExecuteDeleteAsync(ct) > 0;

    public async Task<TodoItem?> Get(Guid Id, CancellationToken ct)
    {
        return await db.Todos.AsNoTracking().FirstOrDefaultAsync(i => i.Id == Id, ct);
    }
}