using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IStudentTestRepository : IGenericRepository<StudentTest>
{
    Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId);

    Task<IEnumerable<StudentTest>> GetByTemplateIdAsync(int templateId);

    Task<StudentTest> GetWithResponsesAsync(int testId);

    Task<StudentTest> GetWithTemplateAsync(int testId);
}