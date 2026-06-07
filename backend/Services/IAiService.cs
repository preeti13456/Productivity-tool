using ExpertOS.API.Models;

namespace ExpertOS.API.Services;

public interface IAiService
{
    Task<string> SummarizeInboxAsync(string? text = null);
    Task<List<ExtractedTask>> ExtractTasksAsync(string? text = null);
    Task<string> AskQuestionAsync(string question);
}
