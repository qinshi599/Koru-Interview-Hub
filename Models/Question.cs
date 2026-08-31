namespace InterviewApp.Models;

public class Question
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Difficulty { get; set; } = string.Empty;
}