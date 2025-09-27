using TaskBoard.Api.Data;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Models;
using TaskStatus = TaskBoard.Api.Models.TaskStatus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var app = builder.Build();



//Add db except for test enviorment
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<TaskBoardContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//get all tasks
app.MapGet("/tasks", async (TaskBoardContext db) =>
{
    var list = await db.Tasks.OrderBy(t => t.CreatedAt).ToListAsync();
    return Results.Ok(list);
});

//get task by id
app.MapGet("/tasks/{id}", async (int id, TaskBoardContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

//create task
app.MapPost("/tasks", async (TaskItemDto dto, TaskBoardContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Title))
        return Results.BadRequest(new { error = "Title is required" });

    var entity = new TaskItem
    {
        Title = dto.Title.Trim(),
        Details = dto.Details,
        Status = dto.Status,
        CreatedAt = DateTime.Now
    };

    db.Tasks.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{entity.Id}", entity);
});

//update task
app.MapPut("/tasks/{id}", async (int id, TaskItemDto dto, TaskBoardContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    if (string.IsNullOrWhiteSpace(dto.Title)) return Results.BadRequest(new { error = "Title is required" });

    task.Title = dto.Title.Trim();
    task.Details = dto.Details;
    task.Status = dto.Status;

    await db.SaveChangesAsync();
    return Results.Ok(task);
});

//delete task
app.MapDelete("/tasks/{id}", async (int id, TaskBoardContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

//DTO
public record TaskItemDto(string Title, string? Details, TaskStatus Status);

public partial class Program {}