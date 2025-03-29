using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IExamTemplateRepository : IGenericRepository<ExamTemplate>
{
    Task<IEnumerable<ExamTemplate>> GetByDisciplineIdAsync(int disciplineId);

    Task<IEnumerable<ExamTemplate>> GetByTeacherIdAsync(string teacherId);

    Task<IEnumerable<ExamTemplate>> GetAllWithDisciplinesAsync();

    Task<ExamTemplate> GetWithQuestionsAsync(int templateId);

    //Task CreateWithRandomQuestionsAsync(ExamTemplate examTemplate);
}