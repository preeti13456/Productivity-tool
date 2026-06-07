using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpertOS.API.Data;
using ExpertOS.API.Models;
using ExpertOS.API.Services;

namespace ExpertOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IAiService _ai;
    private static bool _isDeepWorkMode = false;
    private static string _preferredDeepWorkTime = "9:00 AM - 11:00 AM";

    public DashboardController(AppDbContext db, IAiService ai)
    {
        _db = db;
        _ai = ai;
    }

    [HttpGet("data")]
    public async Task<ActionResult<DashboardData>> GetDashboardData()
    {
        var items = await _db.PriorityStreamItems.ToListAsync();
        var tasks = await _db.ExtractedTasks.ToListAsync();

        var summary = await _ai.SummarizeInboxAsync();

        return Ok(new DashboardData
        {
            PriorityStream = items,
            ExtractedTasks = tasks,
            LiveAiSummary = summary,
            FocusScore = 73,
            TasksCompleted = 8,
            DeepWorkHours = 2.5,
            InterruptionBlocked = 12,
            IsDeepWorkMode = _isDeepWorkMode,
            PreferredDeepWorkTime = _preferredDeepWorkTime
        });
    }

    [HttpPost("summarize-inbox")]
    public async Task<ActionResult<SummarizeResponse>> SummarizeInbox([FromBody] SummarizeRequest req)
    {
        var summary = await _ai.SummarizeInboxAsync(req.Text);
        return Ok(new SummarizeResponse { Summary = summary });
    }

    [HttpPost("extract-tasks")]
    public async Task<ActionResult<ExtractTasksResponse>> ExtractTasks([FromBody] ExtractTasksRequest req)
    {
        var tasks = await _ai.ExtractTasksAsync(req.Text);

        // Persist new tasks to DB
        foreach (var task in tasks)
        {
            task.Id = 0; // let EF assign
            _db.ExtractedTasks.Add(task);
        }
        await _db.SaveChangesAsync();

        return Ok(new ExtractTasksResponse { Tasks = tasks });
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AskResponse>> Ask([FromBody] AskRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Question))
            return BadRequest("Question cannot be empty.");

        var answer = await _ai.AskQuestionAsync(req.Question);
        return Ok(new AskResponse { Answer = answer });
    }

    [HttpPost("prefer-time")]
    public ActionResult PreferTime([FromBody] PreferTimeRequest req)
    {
        _preferredDeepWorkTime = req.Time;
        return Ok(new { message = $"Preferred deep work time set to {req.Time}" });
    }

    [HttpPost("deep-work")]
    public ActionResult<DeepWorkResponse> ToggleDeepWork()
    {
        _isDeepWorkMode = !_isDeepWorkMode;
        return Ok(new DeepWorkResponse
        {
            IsActive = _isDeepWorkMode,
            Message = _isDeepWorkMode
                ? "Deep work mode activated. Notifications paused."
                : "Deep work mode deactivated. Notifications resumed."
        });
    }
}
