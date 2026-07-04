using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public sealed class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponse>> Create(CreateTodoRequest request)
        {
            var createdTodo = await _todoService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TodoResponse>>> GetAll()
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoResponse>> GetById(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoResponse>> Update(int id, UpdateTodoRequest request)
        {
            var updatedTodo = await _todoService.UpdateAsync(id, request);

            if (updatedTodo is null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _todoService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
