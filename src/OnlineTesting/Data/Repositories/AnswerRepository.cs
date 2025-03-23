using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
    {
        return await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .ToListAsync();
    }
}
