using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class ExamTemplateService : IExamTemplateService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExamTemplateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ExamTemplate> GetByIdAsync(int id)
    {
        return await _unitOfWork.ExamTemplates.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ExamTemplate>> GetByDisciplineIdAsync(int disciplineId)
    {
        return await _unitOfWork.ExamTemplates.GetByDisciplineIdAsync(disciplineId);
    }

    public async Task<IEnumerable<ExamTemplate>> GetByTeacherIdAsync(string teacherId)
    {
        return await _unitOfWork.ExamTemplates.GetByTeacherIdAsync(teacherId);
    }

    public async Task<ExamTemplate> GetWithQuestionsAsync(int templateId)
    {
        return await _unitOfWork.ExamTemplates.GetWithQuestionsAsync(templateId);
    }

    public async Task CreateWithRandomQuestionsAsync(ExamTemplate examTemplate)
    {
        //await _unitOfWork.ExamTemplates.CreateWithRandomQuestionsAsync(examTemplate);
        await _unitOfWork.SaveChangesAsync();
    }
}