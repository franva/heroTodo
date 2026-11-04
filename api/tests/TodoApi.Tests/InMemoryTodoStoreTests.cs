using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Tests;

public class InMemoryTodoStoreTests
{
    private readonly InMemoryTodoStore _store = new();
    
    [Fact]
    public void GetAll_IsEmptyInitially()
    {
        Assert.Empty(_store.GetAll());
    }

    [Fact]
    public void Add_StoresTrimmedTitleAndKeepOrder()
    {
        _store.Add(" DragonBall ");
        _store.Add(" One Piece ");

        var titles = _store.GetAll().Select(i => i.Title);
        Assert.Equal(["DragonBall", "One Piece"], titles);
    }

    [Fact]
    public void Remove_ResturnsFalseForUnknownId()
    {
        Assert.False(_store.Remove(Guid.NewGuid()));
    }

    [Fact]
    public void Remove_ReturnsTrueForFOundItem()
    {
        var book = _store.Add(" Dolaamon");
        Assert.True(_store.Remove(book.Id));
    }

    [Fact]
    public void Update_ReturnsUpdatedItem()
    {
        var itemA = _store.Add("Doraamond");

        var newName = itemA.Title + " Gen Z";
        var updatedA = _store.Update(itemA.Id, newName, true);

        Assert.Equal(new TodoItem(itemA.Id, newName, true), updatedA);
    }

    [Fact]
    public void Update_Returns_Null_When_CannotFound()
    {
        var updated = _store.Update(Guid.NewGuid(), ">_<", true);

        Assert.Null(updated);
    }
}
