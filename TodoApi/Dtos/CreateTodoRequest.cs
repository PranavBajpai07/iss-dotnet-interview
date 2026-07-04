using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public sealed class CreateTodoRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }
}
