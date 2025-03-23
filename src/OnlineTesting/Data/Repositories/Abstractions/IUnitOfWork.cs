namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IDisciplineRepository Disciplines { get; }
    IQuestionRepository Questions { get; }
    IAnswerRepository Answers { get; }
    IExamTemplateRepository ExamTemplates { get; }
    IStudentTestRepository StudentTests { get; }
    IStudentResponseRepository StudentResponses { get; }

    Task<int> SaveChangesAsync();
}
