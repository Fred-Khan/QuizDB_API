using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDB_API.Models;

public class QuizHistoryConfiguration : IEntityTypeConfiguration<QuizHistory>
{
    public void Configure(EntityTypeBuilder<QuizHistory> builder)
    {

        // The table name
        builder.ToTable("quizhistory");

        // Primary key and constraint name
        builder.HasKey(e => e.Id)
            .HasName("quizhistory_pkey");

        // id column
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SERIAL")
            .IsRequired();

        // questionid column
        builder.Property(e => e.QuestionId)
            .HasColumnName("questionid")
            .IsRequired();

        // optionname column
        builder.Property(e => e.OptionName)
            .HasColumnName("optionname")
            .HasColumnType("character")
            .HasMaxLength(1)
            .IsRequired();

        // userid column
        builder.Property(e => e.UserId)
            .HasColumnName("userid")
            .IsRequired();

        // Foreign key relationship to QuizUser
        builder.HasOne(h => h.User)
            .WithMany(u => u.History)
            .HasForeignKey(h => h.UserId)
            .HasConstraintName("quizhistory_userid_fkey")
            .IsRequired();
    }
}
