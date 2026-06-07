using ExpertOS.API.Models;

namespace ExpertOS.API.Services;

public class MockAiService : IAiService
{
    public Task<string> SummarizeInboxAsync(string? text = null)
    {
        var summary = "🔴 CRITICAL: Production checkout API is failing for ~30% of users — immediate attention required.\n\n" +
                      "📊 HIGH PRIORITY: CFO needs Q3 revenue projections for board presentation today EOD.\n\n" +
                      "🔧 SPRINT: 3 stories still unmerged ahead of sprint review in 2 hours — coordinate with dev team.\n\n" +
                      "💡 INSIGHT: Enterprise client feature request queued — low urgency but high strategic value.\n\n" +
                      "✅ RECOMMENDATION: Resolve production incident first, then pivot to board deck. Delegate sprint merges.";
        return Task.FromResult(summary);
    }

    public Task<List<ExtractedTask>> ExtractTasksAsync(string? text = null)
    {
        var tasks = new List<ExtractedTask>
        {
            new() { Id = 10, Task = "Deploy hotfix for checkout API 500 errors", Assignee = "Backend Team", Priority = "critical", Source = "AI Extracted", Completed = false },
            new() { Id = 11, Task = "Add Q3 revenue chart to board presentation", Assignee = "You", Priority = "high", Source = "AI Extracted", Completed = false },
            new() { Id = 12, Task = "Review and approve 3 pending PRs before sprint review", Assignee = "You", Priority = "high", Source = "AI Extracted", Completed = false },
            new() { Id = 13, Task = "Schedule discovery call with enterprise client", Assignee = "Jordan Lee", Priority = "medium", Source = "AI Extracted", Completed = false }
        };
        return Task.FromResult(tasks);
    }

    public Task<string> AskQuestionAsync(string question)
    {
        var answer = $"Based on your current context:\n\n" +
                     $"**Q: {question}**\n\n" +
                     "The most critical items right now are the production API incident and the board presentation deadline. " +
                     "I recommend addressing the checkout failure immediately as it impacts revenue directly, " +
                     "then allocate 90 minutes for the Q3 projections. The sprint review can proceed with the 5 already-merged stories if needed.";
        return Task.FromResult(answer);
    }
}
