namespace QuizDB_API.Models;

public class QuizQuestion
{
    public int Id { get; set; }
    public string? QuestionText { get; set; }
    public char OptionName { get; set; }
    public bool Duplicate { get; set; }
    public int PromptId { get; set; }
    public QuizPrompt? Prompt { get; set; } // Navigation property to represent the relationship to QuizPrompt
    public List<QuizOption>? Options { get; set; } // Navigation property to represent the relationship to QuizOption
}
