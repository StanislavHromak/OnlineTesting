using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class AnswerService : IAnswerService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnswerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Answer> GetByIdAsync(int id)
    {
        return await _unitOfWork.Answers.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Answer>> GetAllAsync()
    {
        return await _unitOfWork.Answers.GetAllAsync();
    }

    public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
    {
        return await _unitOfWork.Answers.GetByQuestionIdAsync(questionId);
    }

    public async Task CreateAsync(Answer answer)
    {
        await _unitOfWork.Answers.AddAsync(answer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Answer answer)
    {
        _unitOfWork.Answers.Update(answer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var answer = await _unitOfWork.Answers.GetByIdAsync(id);
        if (answer != null)
        {
            _unitOfWork.Answers.Delete(answer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
