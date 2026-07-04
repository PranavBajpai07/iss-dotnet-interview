using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TodoApi.Models;

namespace TodoApi.Data;

public sealed class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(todo => todo.Id);
            entity.Property(todo => todo.Title).IsRequired().HasMaxLength(200);
            entity.Property(todo => todo.Description).HasMaxLength(1000);
            entity.Property(todo => todo.IsCompleted).HasDefaultValue(false);
            entity.Property(todo => todo.CreatedAt).IsRequired();
        });
    }
}
