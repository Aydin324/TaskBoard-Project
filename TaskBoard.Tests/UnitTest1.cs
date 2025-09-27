using System.Net.Http.Json;
using TaskBoard.Api;
using TaskBoard.Api.Data;
using TaskBoard.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TaskStatus = TaskBoard.Api.Models.TaskStatus;

namespace TaskBoard.Tests;

public class TaskApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public TaskApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_ReturnsOk()
    {
        var response = await _client.GetAsync("/tasks");
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Response Status: " + response.StatusCode);
        Console.WriteLine("Response Body: " + body);

        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>();
        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks); 
    }

    [Fact]
    public async Task GetTask_ById_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TaskBoardContext>();
        var task = db.Tasks.First();

        var response = await _client.GetAsync($"/tasks/{task.Id}");
        response.EnsureSuccessStatusCode();
        var returnedTask = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.Equal(task.Id, returnedTask.Id);
        Assert.Equal(task.Title, returnedTask.Title);
    }

    [Fact]
    public async Task GetTask_NonExisting_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/tasks/9999");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_Valid_ReturnsCreated()
    {
        var dto = new TaskItemDto("New Task", "Details", TaskStatus.Todo);
        var response = await _client.PostAsJsonAsync("/tasks", dto);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.Equal("New Task", created.Title);
    }

    [Fact]
    public async Task CreateTask_Invalid_ReturnsBadRequest()
    {
        var dto = new TaskItemDto("", "Details", TaskStatus.Todo);
        var response = await _client.PostAsJsonAsync("/tasks", dto);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_Valid_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TaskBoardContext>();
        var task = db.Tasks.First();
        var dto = new TaskItemDto("Updated", "Updated details", TaskStatus.Done);

        var response = await _client.PutAsJsonAsync($"/tasks/{task.Id}", dto);
        response.EnsureSuccessStatusCode();

        var updated = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.Equal("Updated", updated.Title);
        Assert.Equal(TaskStatus.Done, updated.Status);
    }

    [Fact]
    public async Task UpdateTask_NonExisting_ReturnsNotFound()
    {
        var dto = new TaskItemDto("Updated", "Details", TaskStatus.Todo);
        var response = await _client.PutAsJsonAsync("/tasks/9999", dto);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_Invalid_ReturnsBadRequest()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TaskBoardContext>();
        var task = db.Tasks.First();
        var dto = new TaskItemDto("", "Details", TaskStatus.Todo);

        var response = await _client.PutAsJsonAsync($"/tasks/{task.Id}", dto);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_Existing_ReturnsNoContent()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TaskBoardContext>();
        var task = db.Tasks.First();

        var response = await _client.DeleteAsync($"/tasks/{task.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        using var checkScope = _factory.Services.CreateScope();
        var checkDb = checkScope.ServiceProvider.GetRequiredService<TaskBoardContext>();
        var deleted = await checkDb.Tasks.FindAsync(task.Id);
        Assert.Null(deleted);
    }


    [Fact]
    public async Task DeleteTask_NonExisting_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/tasks/9999");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
