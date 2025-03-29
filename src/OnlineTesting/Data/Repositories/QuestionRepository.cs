using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IQueryable<Question> GetAll()
    {
        return _context.Questions.AsQueryable();
    }

    public async Task<IEnumerable<Question>> GetByDisciplineIdAsync(int disciplineId)
    {
        return await _context.Questions
            .Where(q => q.DisciplineId == disciplineId)
            .ToListAsync();
    }

    public async Task<Question> GetWithAnswersAsync(int questionId)
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<IEnumerable<Question>> GetRandomQuestionsByDisciplineAsync(int disciplineId, int count)
    {
        return await _context.Questions
            .Where(q => q.DisciplineId == disciplineId)
            .OrderBy(q => Guid.NewGuid())
            .Take(count)
            .ToListAsync();
    }
}