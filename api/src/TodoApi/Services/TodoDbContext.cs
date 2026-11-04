using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class TodoDbContext(DbContextOptions<TodoDbContext> options): DbContext(options)
{
    public DbSet<TodoItem> Todos => Set<TodoItem>();
}