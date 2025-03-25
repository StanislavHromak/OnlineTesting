using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class QuestionService : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork;

    public QuestionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Question> GetByIdAsync(int id)
    {
        return await _unitOfWork.Questions.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await _unitOfWork.Questions.GetAllAsync();
    }

    public async Task<IEnumerable<Question>> GetByDisciplineIdAsync(int disciplineId)
    {
        return await _unitOfWork.Questions.GetByDisciplineIdAsync(disciplineId);
    }

    public async Task<Question> GetWithAnswersAsync(int questionId)
    {
        return await _unitOfWork.Questions.GetWithAnswersAsync(questionId);
    }

    public async Task<IEnumerable<Question>> GetRandomQuestionsByDisciplineAsync(int disciplineId, int count)
    {
        return await _unitOfWork.Questions.GetRandomQuestionsByDisciplineAsync(disciplineId, count);
    }

    public async Task CreateAsync(Question question)
    {
        await _unitOfWork.Questions.AddAsync(question);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Question question)
    {
        _unitOfWork.Questions.Update(question);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var question = await _unitOfWork.Questions.GetByIdAsync(id);
        if (question != null)
        {
            _unitOfWork.Questions.Delete(question);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
