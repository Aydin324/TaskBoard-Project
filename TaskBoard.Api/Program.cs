using TaskBoard.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskBoardContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
var tasks = new List<TaskItem>();
var nextId = 1;

//get all tasks
app.MapGet("/tasks", () => tasks);

//get task by id
app.MapGet("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

//create task
app.MapPost("/tasks", (TaskItem task) =>
{
    var newTask = task with { Id = nextId++, CreatedAt = DateTime.Now };
    tasks.Add(newTask);
    return Results.Created($"/tasks/{newTask.Id}", newTask);
});

//update task
app.MapPut("/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    var taskIndex = tasks.FindIndex(t => t.Id == id);
    if (taskIndex == -1) return Results.NotFound();

    var task = tasks[taskIndex];
    var newTask = updatedTask with { Id = task.Id, CreatedAt = DateTime.Now };
    tasks[taskIndex] = newTask;
    return Results.Ok(newTask);
});

//delete task
app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();
    tasks.Remove(task);
    return Results.NoContent();
});

app.Run();

*/