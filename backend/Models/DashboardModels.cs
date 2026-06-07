using System.Text.Json.Serialization;

namespace ExpertOS.API.Models;

public class PriorityStreamItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; } = "";

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = "";

    [JsonPropertyName("message")]
    public string Message { get; set; } = "";

    [JsonPropertyName("sender")]
    public string Sender { get; set; } = "";

    [JsonPropertyName("time")]
    public string Time { get; set; } = "";

    [JsonPropertyName("isRead")]
    public bool IsRead { get; set; }
}

public class ExtractedTask
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("task")]
    public string Task { get; set; } = "";

    [JsonPropertyName("assignee")]
    public string Assignee { get; set; } = "";

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = "";

    [JsonPropertyName("source")]
    public string Source { get; set; } = "";

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
}

public class DashboardData
{
    [JsonPropertyName("priorityStream")]
    public List<PriorityStreamItem> PriorityStream { get; set; } = new();

    [JsonPropertyName("extractedTasks")]
    public List<ExtractedTask> ExtractedTasks { get; set; } = new();

    [JsonPropertyName("liveAiSummary")]
    public string LiveAiSummary { get; set; } = "";

    [JsonPropertyName("focusScore")]
    public int FocusScore { get; set; }

    [JsonPropertyName("tasksCompleted")]
    public int TasksCompleted { get; set; }

    [JsonPropertyName("deepWorkHours")]
    public double DeepWorkHours { get; set; }

    [JsonPropertyName("interruptionBlocked")]
    public int InterruptionBlocked { get; set; }

    [JsonPropertyName("isDeepWorkMode")]
    public bool IsDeepWorkMode { get; set; }

    [JsonPropertyName("preferredDeepWorkTime")]
    public string PreferredDeepWorkTime { get; set; } = "9:00 AM - 11:00 AM";
}

public class SummarizeRequest
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class SummarizeResponse
{
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = "";
}

public class ExtractTasksRequest
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class ExtractTasksResponse
{
    [JsonPropertyName("tasks")]
    public List<ExtractedTask> Tasks { get; set; } = new();
}

public class AskRequest
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = "";
}

public class AskResponse
{
    [JsonPropertyName("answer")]
    public string Answer { get; set; } = "";
}

public class PreferTimeRequest
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = "";
}

public class DeepWorkResponse
{
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = "";
}
