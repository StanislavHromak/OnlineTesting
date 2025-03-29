using OnlineTesting.Models;
using OnlineTesting.Models.DTOs.StudentTest;
using OnlineTesting.Models.DTOs;

namespace OnlineTesting.Services.Abstractions;

public interface IStudentTestService
{
    Task<StudentTest> GetByIdAsync(int id);
    Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId);
    Task<StudentTest> GetWithResponsesAsync(int testId);
    Task<TestResultDto> GetTestResult(int testId);
    Task<StudentTest> CreateAsync(int templateId, string studentId);
    Task<TakeTestDto> GetCurrentQuestionAsync(int testId);
    Task SaveResponsesAsync(StudentResponseDto responseDto);
    Task MoveToNextQuestionAsync(int testId);
    Task CompleteTestAsync(int testId);
}
