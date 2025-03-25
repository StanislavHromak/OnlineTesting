using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class StudentTestService : IStudentTestService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentTestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentTest> GetByIdAsync(int id)
    {
        return await _unitOfWork.StudentTests.GetByIdAsync(id);
    }

    public async Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId)
    {
        return await _unitOfWork.StudentTests.GetByStudentIdAsync(studentId);
    }

    public async Task<StudentTest> GetWithResponsesAsync(int testId)
    {
        return await _unitOfWork.StudentTests.GetWithResponsesAsync(testId);
    }

    public async Task<int> CalculateScoreAsync(int testId)
    {
        return await _unitOfWork.StudentTests.CalculateScoreAsync(testId);
    }

    public async Task CreateAsync(StudentTest studentTest)
    {
        await _unitOfWork.StudentTests.AddAsync(studentTest);
        await _unitOfWork.SaveChangesAsync();
    }
}
