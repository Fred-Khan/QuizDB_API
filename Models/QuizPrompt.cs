namespace QuizDB_API.Models;

public class QuizPrompt
{
    public int Id { get; set; }
    public string? PromptText { get; set; }
    public List<QuizQuestion>? Questions { get; set; }
}
