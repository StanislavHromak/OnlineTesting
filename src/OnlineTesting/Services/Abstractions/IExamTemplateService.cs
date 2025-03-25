using OnlineTesting.Models;

namespace OnlineTesting.Services.Abstractions;

public interface IExamTemplateService
{
    Task<ExamTemplate> GetByIdAsync(int id);
    Task<IEnumerable<ExamTemplate>> GetByDisciplineIdAsync(int disciplineId);
    Task<IEnumerable<ExamTemplate>> GetByTeacherIdAsync(string teacherId);
    Task<ExamTemplate> GetWithQuestionsAsync(int templateId);
    Task CreateWithRandomQuestionsAsync(ExamTemplate examTemplate);
}