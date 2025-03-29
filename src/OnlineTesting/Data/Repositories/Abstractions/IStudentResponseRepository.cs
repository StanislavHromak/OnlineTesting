using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IStudentResponseRepository : IGenericRepository<StudentResponse>
{
    Task<IEnumerable<StudentResponse>> GetByTestIdAsync(int testId);
    Task<IEnumerable<StudentResponse>> GetByQuestionIdAsync(int questionId);
}