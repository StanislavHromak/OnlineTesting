using OnlineTesting.Models;
using OnlineTesting.Models.DTOs;

namespace OnlineTesting.Services.Abstractions;

public interface IQuestionService
{
    Task<Question> GetByIdAsync(int id);
    Task<IEnumerable<Question>> GetAllAsync();
    Task<IEnumerable<Question>> GetByDisciplineIdAsync(int disciplineId);
    Task<Question> GetWithAnswersAsync(int questionId);
    Task<IEnumerable<Question>> GetRandomQuestionsByDisciplineAsync(int disciplineId, int count);
    Task CreateAsync(Question question);
    Task CreateWithAnswersAsync(QuestionDto questionDto);
    Task UpdateAsync(Question question);
    Task DeleteAsync(int id);
    Task<bool> IsUsedInCompletedTestsAsync(int questionId);
}
