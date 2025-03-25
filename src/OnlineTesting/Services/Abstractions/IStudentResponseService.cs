using OnlineTesting.Models;

namespace OnlineTesting.Services.Abstractions;

public interface IStudentResponseService
{
    Task<StudentResponse> GetByIdAsync(int id);
    Task<IEnumerable<StudentResponse>> GetAllAsync();
    Task<IEnumerable<StudentResponse>> GetByTestIdAsync(int testId);
    Task CreateAsync(StudentResponse studentResponse);
    Task UpdateAsync(StudentResponse studentResponse);
    Task DeleteAsync(int id);
}
