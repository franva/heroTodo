using Microsoft.EntityFrameworkCore;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Todo") ?? "Data Source=todos.db"));
builder.Services.AddScoped<ITodoStore, EfTodoStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TodoDbContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;