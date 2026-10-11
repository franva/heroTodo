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
}
