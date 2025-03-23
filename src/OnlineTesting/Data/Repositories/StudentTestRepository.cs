using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class StudentTestRepository : GenericRepository<StudentTest>, IStudentTestRepository
{
    public StudentTestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId)
    {
        return await _context.StudentTests
            .Where(st => st.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<StudentTest>> GetByTemplateIdAsync(int templateId)
    {
        return await _context.StudentTests
            .Where(st => st.TemplateId == templateId)
            .ToListAsync();
    }

    public async Task<StudentTest> GetWithResponsesAsync(int testId)
    {
        return await _context.StudentTests
            .Include(st => st.StudentResponses)
                .ThenInclude(sr => sr.Question)
            .Include(st => st.StudentResponses)
                .ThenInclude(sr => sr.Answer)
            .FirstOrDefaultAsync(st => st.Id == testId);
    }

    public async Task<int> CalculateScoreAsync(int testId)
    {
        var responses = await _context.StudentResponses
            .Include(sr => sr.Answer)
            .Where(sr => sr.TestId == testId)
            .ToListAsync();

        return responses.Sum(r => r.Answer.IsCorrect ? 1 : 0);
    }
}