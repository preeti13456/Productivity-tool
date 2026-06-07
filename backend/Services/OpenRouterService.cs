using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpertOS.API.Models;

namespace ExpertOS.API.Services;

public class OpenRouterService : IAiService
{
    private readonly HttpClient _http;
    private readonly string _model;
    private readonly ILogger<OpenRouterService> _logger;

    public OpenRouterService(IConfiguration config, ILogger<OpenRouterService> logger)
    {
        _logger = logger;
        var apiKey = config["OpenRouter:ApiKey"] ?? "";
        var baseUrl = config["OpenRouter:BaseUrl"] ?? "https://openrouter.ai/api/v1";
        _model = config["OpenRouter:Model"] ?? "openai/gpt-4o-mini";

        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.Add("HTTP-Referer", "https://expertosdash.app");
        _http.DefaultRequestHeaders.Add("X-Title", "ExpertOS Dashboard");
    }

    private async Task<string> ChatAsync(string systemPrompt, string userPrompt)
    {
        var payload = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user",   content = userPrompt }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var response = await _http.PostAsync("/chat/completions",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(body);
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";
    }

    public async Task<string> SummarizeInboxAsync(string? text = null)
    {
        var system = "You are an intelligent productivity assistant. Summarize the user's inbox/messages concisely, highlighting critical items, action items, and recommended priorities. Use emojis for visual clarity.";
        var user = string.IsNullOrWhiteSpace(text)
            ? "Summarize my current work inbox. Include: production issues, deadlines, team blockers, and recommendations."
            : $"Summarize the following messages:\n\n{text}";

        return await ChatAsync(system, user);
    }

    public async Task<List<ExtractedTask>> ExtractTasksAsync(string? text = null)
    {
        var system = "You are a task extraction AI. Extract actionable tasks from the provided text. Return ONLY valid JSON array, no markdown. Each task: {\"id\": number, \"task\": string, \"assignee\": string, \"priority\": \"critical|high|medium|low\", \"source\": \"AI Extracted\", \"completed\": false}";
        var user = string.IsNullOrWhiteSpace(text)
            ? "Extract tasks from: Production API failing on checkout. Board presentation due EOD. Sprint review in 2 hours with 3 unmerged stories."
            : $"Extract tasks from:\n\n{text}";

        try
        {
            var result = await ChatAsync(system, user);
            // Strip any markdown fences if present
            result = result.Trim().TrimStart('`');
            if (result.StartsWith("json")) result = result[4..];
            result = result.TrimEnd('`').Trim();
            return JsonSerializer.Deserialize<List<ExtractedTask>>(result) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse task extraction response");
            return new List<ExtractedTask>
            {
                new() { Id = 1, Task = "Review and prioritize inbox items", Assignee = "You", Priority = "high", Source = "AI Extracted", Completed = false }
            };
        }
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        var system = "You are ExpertOS, an intelligent productivity assistant. You help users manage tasks, priorities, and deep work. Be concise, actionable, and insightful.";
        return await ChatAsync(system, question);
    }
}
