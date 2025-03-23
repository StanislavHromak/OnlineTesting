using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<IEnumerable<Question>> GetByDisciplineIdAsync(int disciplineId);

    Task<Question> GetWithAnswersAsync(int questionId);

    Task<IEnumerable<Question>> GetRandomQuestionsByDisciplineAsync(int disciplineId, int count);
}
