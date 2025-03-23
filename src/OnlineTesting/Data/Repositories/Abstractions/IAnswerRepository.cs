using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
}