namespace QuizDB_API.Models;

public class QuizHistory
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public char OptionName { get; set; }
    public int UserId { get; set; }
    public QuizUser? User { get; set; } // Navigation property to represent the relationship to QuizUser
}
