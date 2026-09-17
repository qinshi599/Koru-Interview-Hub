namespace InterviewApp.Models;

public class Question
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Difficulty { get; set; } = string.Empty;

    //foreign key to Category
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
     public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
}