using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Models;

namespace TaskBoard.Api.Data;

public class TaskBoardContext : DbContext
{
    public TaskBoardContext(DbContextOptions<TaskBoardContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks { get; set; }
}