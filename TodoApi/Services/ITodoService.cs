using TodoApi.Dtos;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<TodoResponse> CreateAsync(CreateTodoRequest request);
    Task<IReadOnlyList<TodoResponse>> GetAllAsync();
    Task<TodoResponse?> GetByIdAsync(int id);
    Task<TodoResponse?> UpdateAsync(int id, UpdateTodoRequest request);
    Task<bool> DeleteAsync(int id);
}
