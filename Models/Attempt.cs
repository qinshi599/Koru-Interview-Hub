namespace InterviewApp.Models;

public class Attempt
{
    public int Id { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string AnswerText { get; set; } = string.Empty;
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

    // AI-generated composite score (0-100), null until scored.
    public int? Score { get; set; }

    // AI-generated structured feedback text, null until scored.
    public string? Feedback { get; set; }
}
