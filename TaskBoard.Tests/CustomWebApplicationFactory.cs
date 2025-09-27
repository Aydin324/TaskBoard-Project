using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Api;
using TaskBoard.Api.Data;
using TaskBoard.Api.Models;
using TaskStatus = TaskBoard.Api.Models.TaskStatus;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<TaskBoardContext>))
                .ToList();
            foreach (var d in descriptors)
            services.Remove(d);

            services.AddDbContext<TaskBoardContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TaskBoardContext>();
            db.Database.EnsureCreated();

            if (!db.Tasks.Any())
            {
                 db.Tasks.Add(new TaskItem
                {
                    Title = "Sample task",
                    Status = TaskStatus.Todo
                });
                db.SaveChanges();
            }
        });

    }
}
