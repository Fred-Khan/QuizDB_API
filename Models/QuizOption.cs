namespace QuizDB_API.Models;

public class QuizOption
{
    public int Id { get; set; }
    public string? OptionName { get; set; }
    public string? OptionText { get; set; }
    public int QuestionId { get; set; }
    public QuizQuestion? Question { get; set; } // Navigation property to represent the relationship to QuizQuestion
}
