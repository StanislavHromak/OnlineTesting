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
            .Where(t => t.TemplateId == templateId)
            .Include(t => t.Student)
            .ToListAsync();
    }

    public async Task<StudentTest> GetWithResponsesAsync(int testId)
    {
        return await _context.StudentTests
            .Include(t => t.StudentResponses)
                .ThenInclude(sr => sr.Question)
            .Include(t => t.StudentResponses)
                .ThenInclude(sr => sr.SelectedAnswers)
            .FirstOrDefaultAsync(st => st.Id == testId);
    }

    public async Task<StudentTest> GetWithTemplateAsync(int testId)
    {
        return await _context.StudentTests
            .Include(t => t.ExamTemplate)
            .FirstOrDefaultAsync(st => st.Id == testId);
    }
}