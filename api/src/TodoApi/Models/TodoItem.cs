namespace TodoApi.Models;

public record TodoItem(Guid Id, string Title, bool IsCompleted = false);