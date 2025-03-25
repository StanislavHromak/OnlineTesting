using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class StudentResponseService : IStudentResponseService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentResponseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentResponse> GetByIdAsync(int id)
    {
        return await _unitOfWork.StudentResponses.GetByIdAsync(id);
    }

    public async Task<IEnumerable<StudentResponse>> GetAllAsync()
    {
        return await _unitOfWork.StudentResponses.GetAllAsync();
    }

    public async Task<IEnumerable<StudentResponse>> GetByTestIdAsync(int testId)
    {
        return await _unitOfWork.StudentResponses.GetByTestIdAsync(testId);
    }

    public async Task CreateAsync(StudentResponse studentResponse)
    {
        await _unitOfWork.StudentResponses.AddAsync(studentResponse);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudentResponse studentResponse)
    {
        _unitOfWork.StudentResponses.Update(studentResponse);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var studentResponse = await _unitOfWork.StudentResponses.GetByIdAsync(id);
        if (studentResponse != null)
        {
            _unitOfWork.StudentResponses.Delete(studentResponse);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}