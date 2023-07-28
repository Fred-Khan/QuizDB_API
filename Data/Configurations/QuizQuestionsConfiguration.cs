using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDB_API.Models;

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {

        // The table name
        builder.ToTable("quizquestions");

        // Primary key and constraint name
        builder.HasKey(e => e.Id)
            .HasName("quizquestions_pkey");

        // id column
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SERIAL")
            .IsRequired();

        // questiontext column
        builder.Property(e => e.QuestionText)
            .HasColumnName("questiontext")
            .HasColumnType("character varying")
            .IsRequired();

        // optionname column
        builder.Property(e => e.OptionName)
            .HasColumnName("optionname")
            .HasColumnType("character")
            .HasMaxLength(1)
            .IsRequired();

        // duplicate column
        builder.Property(e => e.Duplicate)
            .HasColumnName("duplicate")
            .IsRequired();

        // promptid column
        builder.Property(e => e.PromptId)
            .HasColumnName("promptid")
            .IsRequired();

        // Foreign key relationship to QuizPrompt
        builder.HasOne(q => q.Prompt)
            .WithMany(p => p.Questions)
            .HasForeignKey(q => q.PromptId)
            .HasConstraintName("quizquestions_promptid_fkey")
            .IsRequired();
    }
}
