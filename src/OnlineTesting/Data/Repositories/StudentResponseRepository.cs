using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class StudentResponseRepository : GenericRepository<StudentResponse>, IStudentResponseRepository
{
    public StudentResponseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StudentResponse>> GetByTestIdAsync(int testId)
    {
        return await _context.StudentResponses
            .Where(sr => sr.TestId == testId)
            .ToListAsync();
    }
}
