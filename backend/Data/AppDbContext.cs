using Microsoft.EntityFrameworkCore;
using ExpertOS.API.Models;

namespace ExpertOS.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PriorityStreamItem> PriorityStreamItems { get; set; }
    public DbSet<ExtractedTask> ExtractedTasks { get; set; }

    public void SeedData()
    {
        if (!PriorityStreamItems.Any())
        {
            PriorityStreamItems.AddRange(
                new PriorityStreamItem { Id = 1, Source = "Slack", Priority = "critical", Message = "Production API is throwing 500 errors on checkout flow - affects 30% of users", Sender = "Alex Chen", Time = "2 min ago", IsRead = false },
                new PriorityStreamItem { Id = 2, Source = "Email", Priority = "high", Message = "Q3 board presentation needs revenue projections by EOD - CFO request", Sender = "Sarah Kim", Time = "15 min ago", IsRead = false },
                new PriorityStreamItem { Id = 3, Source = "Jira", Priority = "high", Message = "Sprint review in 2 hours - 3 stories still not merged to main", Sender = "Dev Team", Time = "1 hr ago", IsRead = true },
                new PriorityStreamItem { Id = 4, Source = "Slack", Priority = "medium", Message = "New feature request from enterprise client - needs timeline estimate", Sender = "Jordan Lee", Time = "2 hr ago", IsRead = true },
                new PriorityStreamItem { Id = 5, Source = "Email", Priority = "low", Message = "Monthly all-hands scheduled for next Tuesday 3pm", Sender = "HR Team", Time = "3 hr ago", IsRead = true }
            );

            ExtractedTasks.AddRange(
                new ExtractedTask { Id = 1, Task = "Investigate and fix checkout API 500 errors", Assignee = "Backend Team", Priority = "critical", Source = "Slack", Completed = false },
                new ExtractedTask { Id = 2, Task = "Prepare Q3 revenue projections for board deck", Assignee = "You", Priority = "high", Source = "Email", Completed = false },
                new ExtractedTask { Id = 3, Task = "Merge remaining 3 sprint stories before review", Assignee = "Dev Team", Priority = "high", Source = "Jira", Completed = false },
                new ExtractedTask { Id = 4, Task = "Send timeline estimate to Jordan for enterprise feature", Assignee = "You", Priority = "medium", Source = "Slack", Completed = false }
            );

            SaveChanges();
        }
    }
}
