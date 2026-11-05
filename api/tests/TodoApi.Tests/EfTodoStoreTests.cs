using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class EfTodoStoreTests : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly TodoDbContext _db;
    private readonly EfTodoStore _store;
    private static CancellationToken Ct => CancellationToken.None;

    public EfTodoStoreTests()
    {
        _connection.Open();   // an in-memory SQLite database lives only while its connection stays open
        _db = new TodoDbContext(new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options);
        _db.Database.EnsureCreated();
        _store = new EfTodoStore(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    // [Fact]
    // public void GetAll_IsEmptyInitially()
    // {
    //     Assert.Empty(_store.GetAll(null));
    // }

    // [Fact]
    // public void Add_StoresTrimmedTitleAndKeepOrder()
    // {
    //     _store.Add(" DragonBall ");
    //     _store.Add(" One Piece ");

    //     var titles = _store.GetAll().Select(i => i.Title);
    //     Assert.Equal(["DragonBall", "One Piece"], titles);
    // }

    // [Fact]
    // public void Remove_ResturnsFalseForUnknownId()
    // {
    //     Assert.False(_store.Remove(Guid.NewGuid()));
    // }

    // [Fact]
    // public void Remove_ReturnsTrueForFOundItem()
    // {
    //     var book = _store.Add(" Dolaamon");
    //     Assert.True(_store.Remove(book.Id));
    // }

    // [Fact]
    // public void Update_ReturnsUpdatedItem()
    // {
    //     var itemA = _store.Add("Doraamond");

    //     var newName = itemA.Title + " Gen Z";
    //     var updatedA = _store.Update(itemA.Id, newName, true);

    //     Assert.Equal(new TodoItem(itemA.Id, newName, true), updatedA);
    // }

    // [Fact]
    // public void Update_Returns_Null_When_CannotFound()
    // {
    //     var updated = _store.Update(Guid.NewGuid(), ">_<", true);

    //     Assert.Null(updated);
    // }
}
