using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.Models;
using Xunit.Sdk;

namespace TodoApi.Tests;

public class TodosApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string apiBase = "api/todos";
    private HttpClient TestClient() => factory.WithWebHostBuilder(_ => {}).CreateClient();

    [Fact]
    public async Task Post_CreatesItem()
    {
        var client = TestClient();
        var res = await client.PostAsJsonAsync(apiBase, new {title = "buy book"});
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);

        var created = await res.Content.ReadFromJsonAsync<TodoItem>();
        var all = await client.GetFromJsonAsync<List<TodoItem>>("/api/todos");
        Assert.Equal(created, Assert.Single(all!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Post_RejectedEmptyTitle(string title)
    {
        var res = await TestClient().PostAsJsonAsync(apiBase, new {title});
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Post_RejectsTitleOver200Chars()
    {
        var res = await TestClient().PostAsJsonAsync("/api/todos", new { title = new string('O', 201) });

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Delete_RemovesItem()
    {
        var client = TestClient();
        var created = await (await client.PostAsJsonAsync("/api/todos", new { title = "x" }))
            .Content.ReadFromJsonAsync<TodoItem>();

        var res = await client.DeleteAsync($"/api/todos/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);
        Assert.Empty(await client.GetFromJsonAsync<List<TodoItem>>("/api/todos") ?? []);
    }

    [Fact]
    public async Task Delete_UnknownId_Returns404()
    {
        var res = await TestClient().DeleteAsync($"/api/todos/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    
    [Fact]
    public async Task Post_ReturnsLocationThatCanBeFetched()
    {
        var client = TestClient();
        var response = await client.PostAsJsonAsync(apiBase, new {title = "test"});
        var fetched = await client.GetAsync(response.Headers.Location);

        Assert.Equal(HttpStatusCode.OK, fetched.StatusCode);
    }
}