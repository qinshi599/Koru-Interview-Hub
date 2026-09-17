namespace InterviewApp.Models;

public class Attempt
{
    public int Id { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string AnswerText { get; set; } = string.Empty;
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
}
