using OnlineTesting.Models;

namespace OnlineTesting.Services.Abstractions;

public interface IStudentTestService
{
    Task<StudentTest> GetByIdAsync(int id);
    Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId);
    Task<StudentTest> GetWithResponsesAsync(int testId);
    Task<int> CalculateScoreAsync(int testId);
    Task CreateAsync(StudentTest studentTest);
}
