using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDB_API.Models;

public class QuizPromptConfiguration : IEntityTypeConfiguration<QuizPrompt>
{
    public void Configure(EntityTypeBuilder<QuizPrompt> builder)
    {

        // The table name
        builder.ToTable("quizprompts");
        
        // Primary key and constraint name
        builder.HasKey(e => e.Id)
            .HasName("quizprompts_pkey");

        // id column
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SERIAL")
            .IsRequired();

        // prompttext column
        builder.Property(e => e.PromptText)
            .HasColumnName("prompttext")
            .HasColumnType("character varying")
            .IsRequired();
    }
}
