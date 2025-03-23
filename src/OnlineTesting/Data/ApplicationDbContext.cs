using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineTesting.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineTesting.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Discipline> Disciplines { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<ExamTemplate> ExamTemplates { get; set; }
    public DbSet<StudentTest> StudentTests { get; set; }
    public DbSet<StudentResponse> StudentResponses { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExamTemplate>()
            .HasMany(et => et.Questions)
            .WithMany(q => q.ExamTemplates)
            .UsingEntity<Dictionary<string, object>>("ExamTemplateQuestions",
                j => j.HasOne<Question>().WithMany().HasForeignKey("QuestionsId").OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<ExamTemplate>().WithMany().HasForeignKey("ExamTemplatesId").OnDelete(DeleteBehavior.Cascade));

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Discipline)
            .WithMany(d => d.Questions)
            .HasForeignKey(q => q.DisciplineId);

        modelBuilder.Entity<ExamTemplate>()
            .HasOne(et => et.Discipline)
            .WithMany(d => d.ExamTemplates)
            .HasForeignKey(et => et.DisciplineId);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId);

        modelBuilder.Entity<ExamTemplate>()
            .HasOne(et => et.Teacher)
            .WithMany(u => u.ExamTemplates)
            .HasForeignKey(et => et.TeacherId);

        modelBuilder.Entity<StudentTest>()
            .HasOne(st => st.ExamTemplate)
            .WithMany(et => et.StudentTests)
            .HasForeignKey(st => st.TemplateId);

        modelBuilder.Entity<StudentTest>()
            .HasOne(st => st.Student)
            .WithMany(u => u.StudentTests)
            .HasForeignKey(st => st.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentResponse>()
            .HasOne(sr => sr.StudentTest)
            .WithMany(st => st.StudentResponses)
            .HasForeignKey(sr => sr.TestId);

        modelBuilder.Entity<StudentResponse>()
            .HasOne(sr => sr.Question)
            .WithMany(q => q.StudentResponses)
            .HasForeignKey(sr => sr.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentResponse>()
            .HasOne(sr => sr.Answer)
            .WithMany(a => a.StudentResponses)
            .HasForeignKey(sr => sr.AnswerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}