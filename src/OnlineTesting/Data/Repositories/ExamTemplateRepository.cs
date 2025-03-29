using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class ExamTemplateRepository : GenericRepository<ExamTemplate>, IExamTemplateRepository
{
    private readonly IQuestionRepository _questionRepository;

    public ExamTemplateRepository(ApplicationDbContext context, IQuestionRepository questionRepository)
        : base(context)
    {
        _questionRepository = questionRepository;
    }

    public async Task<IEnumerable<ExamTemplate>> GetByDisciplineIdAsync(int disciplineId)
    {
        return await _context.ExamTemplates
            .Where(et => et.DisciplineId == disciplineId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExamTemplate>> GetByTeacherIdAsync(string teacherId)
    {
        return await _context.ExamTemplates
            .Where(et => et.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExamTemplate>> GetAllWithDisciplinesAsync()
    {
        return await _context.ExamTemplates
            .Include(et => et.Discipline)
            .ToListAsync();
    }

    public async Task<ExamTemplate> GetWithQuestionsAsync(int templateId)
    {
        return await _context.ExamTemplates
            .Include(et => et.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(et => et.Id == templateId);
    }

    /*public async Task CreateWithRandomQuestionsAsync(ExamTemplate examTemplate)
    {
        examTemplate.CreatedAt = DateTime.UtcNow;
        await _context.ExamTemplates.AddAsync(examTemplate);
        await _context.SaveChangesAsync();

        var questions = await _questionRepository.GetRandomQuestionsByDisciplineAsync(
            examTemplate.DisciplineId,
            examTemplate.NumberOfQuestions
        );

        if (questions.Count() < examTemplate.NumberOfQuestions)
        {
            throw new InvalidOperationException(
                $"Not enough questions available for DisciplineId {examTemplate.DisciplineId}. " +
                $"Required: {examTemplate.NumberOfQuestions}, Found: {questions.Count()}"
            );
        }

        examTemplate.Questions = questions.ToList();
    }*/
}