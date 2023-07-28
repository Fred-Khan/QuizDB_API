using Microsoft.EntityFrameworkCore;
using QuizDB_API.Models;
public partial class QuizDbContext : DbContext
{
    // Default constructor
    public QuizDbContext()
    {
    }

    // Constructor with DbContextOptions parameter
    public QuizDbContext(DbContextOptions<QuizDbContext> options)
        : base(options)
    {
    }

    public DbSet<QuizPrompt>? QuizPrompts { get; set; }
    public DbSet<QuizQuestion>? QuizQuestions { get; set; }
    public DbSet<QuizOption>? QuizOptions { get; set; }
    public DbSet<QuizUser>? QuizUsers { get; set; }
    public DbSet<QuizHistory>? QuizHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Apply the QuizPrompts entity/table configurations
        modelBuilder.ApplyConfiguration(new QuizPromptConfiguration());

        // Apply the QuizQuestions entity/table configurations
        modelBuilder.ApplyConfiguration(new QuizQuestionConfiguration());

        // Apply the QuizOptions entity/table configurations
        modelBuilder.ApplyConfiguration(new QuizOptionConfiguration());

        // Apply the QuizUsers entity/table configurations
        modelBuilder.ApplyConfiguration(new QuizUserConfiguration());

        // Apply the QuizHistory entity/table configurations
        modelBuilder.ApplyConfiguration(new QuizHistoryConfiguration());
        
        // Define any additional configurations for your entities, if necessary.
    }
}
