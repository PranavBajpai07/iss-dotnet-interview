using System.Net;
using System.Net.Http.Json;
using TodoApi.Dtos;
using TodoApi.Tests.TestHelpers;
using Xunit;

namespace TodoApi.Tests.Api;

public sealed class TodosApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TodosApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTodo_WithValidRequest_ReturnsCreatedTodo()
    {
        var request = new CreateTodoRequest
        {
            Title = "Prepare solution",
            Description = "Refactor TODO API"
        };

        var response = await _client.PostAsJsonAsync("/api/todos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todo = await response.Content.ReadFromJsonAsync<TodoResponse>();
        Assert.NotNull(todo);
        Assert.True(todo!.Id > 0);
        Assert.Equal("Prepare solution", todo.Title);
        Assert.Equal("Refactor TODO API", todo.Description);
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public async Task CreateTodo_WithEmptyTitle_ReturnsBadRequest()
    {
        var request = new CreateTodoRequest
        {
            Title = string.Empty,
            Description = "Invalid request"
        };

        var response = await _client.PostAsJsonAsync("/api/todos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTodos_ReturnsTodos()
    {
        await CreateTodoAsync("First todo", "First description");
        await CreateTodoAsync("Second todo", "Second description");

        var todos = await _client.GetFromJsonAsync<List<TodoResponse>>("/api/todos");

        Assert.NotNull(todos);
        Assert.Contains(todos!, todo => todo.Title == "First todo");
        Assert.Contains(todos!, todo => todo.Title == "Second todo");
    }

    [Fact]
    public async Task GetTodoById_WithExistingId_ReturnsTodo()
    {
        var created = await CreateTodoAsync("Find me", "Lookup by id");

        var todo = await _client.GetFromJsonAsync<TodoResponse>($"/api/todos/{created.Id}");

        Assert.NotNull(todo);
        Assert.Equal(created.Id, todo!.Id);
        Assert.Equal("Find me", todo.Title);
    }

    [Fact]
    public async Task GetTodoById_WithMissingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/todos/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTodo_WithExistingId_ReturnsUpdatedTodo()
    {
        var created = await CreateTodoAsync("Old title", "Old description");
        var request = new UpdateTodoRequest
        {
            Title = "Updated title",
            Description = "Updated description",
            IsCompleted = true
        };

        var response = await _client.PutAsJsonAsync($"/api/todos/{created.Id}", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content.ReadFromJsonAsync<TodoResponse>();
        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated!.Id);
        Assert.Equal("Updated title", updated.Title);
        Assert.Equal("Updated description", updated.Description);
        Assert.True(updated.IsCompleted);
    }

    [Fact]
    public async Task UpdateTodo_WithMissingId_ReturnsNotFound()
    {
        var request = new UpdateTodoRequest
        {
            Title = "Missing todo",
            Description = "Should return not found",
            IsCompleted = true
        };

        var response = await _client.PutAsJsonAsync("/api/todos/999999", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WithExistingId_ReturnsNoContentAndRemovesTodo()
    {
        var created = await CreateTodoAsync("Delete me", "Delete test");

        var deleteResponse = await _client.DeleteAsync($"/api/todos/{created.Id}");
        var getResponse = await _client.GetAsync($"/api/todos/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WithMissingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/todos/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<TodoResponse> CreateTodoAsync(string title, string description)
    {
        var response = await _client.PostAsJsonAsync("/api/todos", new CreateTodoRequest
        {
            Title = title,
            Description = description
        });

        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<TodoResponse>();
        return created!;
    }
}
