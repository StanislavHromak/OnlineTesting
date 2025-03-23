using OnlineTesting.Data.Repositories.Abstractions;

namespace OnlineTesting.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDisciplineRepository _disciplines;
    private IQuestionRepository _questions;
    private IAnswerRepository _answers;
    private IExamTemplateRepository _examTemplates;
    private IStudentTestRepository _studentTests;
    private IStudentResponseRepository _studentResponses;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IDisciplineRepository Disciplines => _disciplines ??= new DisciplineRepository(_context);
    public IQuestionRepository Questions => _questions ??= new QuestionRepository(_context);
    public IAnswerRepository Answers => _answers ??= new AnswerRepository(_context);
    public IExamTemplateRepository ExamTemplates => _examTemplates ??= new ExamTemplateRepository(_context, Questions);
    public IStudentTestRepository StudentTests => _studentTests ??= new StudentTestRepository(_context);
    public IStudentResponseRepository StudentResponses => _studentResponses ??= new StudentResponseRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}