using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizDB_API.Models;

public class QuizUserConfiguration : IEntityTypeConfiguration<QuizUser>
{
    public void Configure(EntityTypeBuilder<QuizUser> builder)
    {
        // The table name
        builder.ToTable("quizusers");

        // Primary key and constraint name
        builder.HasKey(e => e.Id)
            .HasName("quizusers_pkey");

        // id column
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SERIAL")
            .IsRequired();

        // Username column with unique constraint
        builder.Property(e => e.UserName)
            .HasColumnName("username")
            .HasColumnType("character varying")
            .HasMaxLength(255)
            .IsRequired();
        // Unique constraint on UserName column
        builder.HasIndex(e => e.UserName)
            .IsUnique()
            .HasDatabaseName("quizusers_username_key");

        // FirstName column
        builder.Property(e => e.FirstName)
            .HasColumnName("firstname")
            .HasColumnType("character varying")
            .HasMaxLength(255)
            .IsRequired();

        // LastName column
        builder.Property(e => e.LastName)
            .HasColumnName("lastname")
            .HasColumnType("character varying")
            .HasMaxLength(255)
            .IsRequired();
    }
}
