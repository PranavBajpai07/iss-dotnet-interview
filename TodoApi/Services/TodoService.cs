using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Services;

public sealed class TodoService : ITodoService
{
    private readonly TodoDbContext _dbContext;

    public TodoService(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TodoResponse> CreateAsync(CreateTodoRequest request)
    {
        var todo = new Todo
        {
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();
        return MapToResponse(todo);
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAllAsync()
    {
        return await _dbContext.Todos
            .AsNoTracking()
            .OrderBy(todo => todo.Id)
            .Select(todo => MapToResponse(todo))
            .ToListAsync();
    }

    public async Task<TodoResponse?> GetByIdAsync(int id)
    {
        var todo = await _dbContext.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return todo is null ? null : MapToResponse(todo);
    }

    public async Task<TodoResponse?> UpdateAsync(int id, UpdateTodoRequest request)
    {
        var todo = await _dbContext.Todos.FirstOrDefaultAsync(item => item.Id == id);

        if (todo is null)
        {
            return null;
        }

        todo.Title = request.Title.Trim();
        todo.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        todo.IsCompleted = request.IsCompleted;

        await _dbContext.SaveChangesAsync();
        return MapToResponse(todo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _dbContext.Todos.FirstOrDefaultAsync(item => item.Id == id);

        if (todo is null)
        {
            return false;
        }

        _dbContext.Todos.Remove(todo);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private static TodoResponse MapToResponse(Todo todo)
    {
        return new TodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CreatedAt = todo.CreatedAt
        };
    }
}
