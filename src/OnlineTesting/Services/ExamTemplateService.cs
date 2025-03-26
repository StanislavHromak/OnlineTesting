using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Models.DTOs.ExamTemplate;
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

    public async Task CreateWithRandomQuestionsAsync(ExamTemplateDto templateDto)
    {
        var template = new ExamTemplate
        {
            Name = templateDto.Name,
            DisciplineId = templateDto.DisciplineId,
            NumberOfQuestions = templateDto.QuestionCount,
            TimeLimit = templateDto.DurationMinutes,
            TeacherId = templateDto.TeacherId
        };

        // Вибираємо випадкові питання
        var randomQuestions = await _unitOfWork.Questions.GetRandomQuestionsByDisciplineAsync(template.DisciplineId, template.NumberOfQuestions);
        template.Questions = randomQuestions.ToList();

        // Перевіряємо, чи достатньо питань
        if (template.Questions.Count < template.NumberOfQuestions)
        {
            throw new InvalidOperationException($"Недостатньо питань для дисципліни з ID {template.DisciplineId}. Потрібно {template.NumberOfQuestions}, але знайдено лише {template.Questions.Count}.");
        }

        await _unitOfWork.ExamTemplates.AddAsync(template);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(ExamTemplate examTemplate)
    {
        _unitOfWork.ExamTemplates.Update(examTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var examTemplate = await _unitOfWork.ExamTemplates.GetByIdAsync(id);
        if (examTemplate != null)
        {
            _unitOfWork.ExamTemplates.Delete(examTemplate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}