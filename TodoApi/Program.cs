using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.EnsureCreated();
}

//InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}


//void InitializeDatabase()
//{
//    var connectionString = "Data Source=todos.db";
//    using var connection = new SqliteConnection(connectionString);
//    connection.Open();

//    var command = connection.CreateCommand();
//    command.CommandText = @"
//        CREATE TABLE IF NOT EXISTS Todos (
//            Id INTEGER PRIMARY KEY AUTOINCREMENT,
//            Title TEXT NOT NULL,
//            Description TEXT,
//            IsCompleted INTEGER NOT NULL DEFAULT 0,
//            CreatedAt TEXT NOT NULL
//        )
//    ";
//    command.ExecuteNonQuery();

//    Console.WriteLine("Database initialized successfully");
//}
