using OnlineTesting.Models;

namespace OnlineTesting.Services.Abstractions;

public interface IAnswerService
{
    Task<Answer> GetByIdAsync(int id);
    Task<IEnumerable<Answer>> GetAllAsync();
    Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
    Task CreateAsync(Answer answer);
    Task UpdateAsync(Answer answer);
    Task DeleteAsync(int id);
}
