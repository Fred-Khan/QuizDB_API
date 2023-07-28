namespace QuizDB_API.Models;

public class QuizUser
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<QuizHistory>? History { get; set; }
}
