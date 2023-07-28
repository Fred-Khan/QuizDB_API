using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDB_API.Models;

public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOption>
{
    public void Configure(EntityTypeBuilder<QuizOption> builder)
    {

        // The table name
        builder.ToTable("quizoptions");
        
        // Primary key and constraint name
        builder.HasKey(e => e.Id)
            .HasName("quizoptions_pkey");

        // id column
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SERIAL")
            .IsRequired();

        // optionname column
        builder.Property(e => e.OptionName)
            .HasColumnName("optionname")
            .HasColumnType("character")
            .HasMaxLength(1)
            .IsRequired();

        // optiontext column
        builder.Property(e => e.OptionText)
            .HasColumnName("optiontext")
            .HasColumnType("character varying")
            .IsRequired();

        // questionid column
        builder.Property(e => e.QuestionId)
            .HasColumnName("questionid")
            .IsRequired();

        // Foreign key relationship to QuizQuestion
        builder.HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .HasConstraintName("quizoptions_questionid_fkey")
            .IsRequired();
    }
}
