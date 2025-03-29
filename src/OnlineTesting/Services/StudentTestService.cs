using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Models.DTOs;
using OnlineTesting.Models.DTOs.StudentTest;
using OnlineTesting.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace OnlineTesting.Services;

public class StudentTestService : IStudentTestService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentTestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentTest> GetByIdAsync(int id)
    {
        return await _unitOfWork.StudentTests.GetByIdAsync(id);
    }

    public async Task<IEnumerable<StudentTest>> GetByStudentIdAsync(string studentId)
    {
        return await _unitOfWork.StudentTests.GetByStudentIdAsync(studentId);
    }

    public async Task<StudentTest> GetWithResponsesAsync(int testId)
    {
        return await _unitOfWork.StudentTests.GetWithResponsesAsync(testId);
    }

    public async Task<TestResultDto> GetTestResult(int testId)
    {
        var test = await _unitOfWork.StudentTests.GetWithTemplateAsync(testId);
        if (test == null)
        {
            throw new InvalidOperationException("Тест не знайдено.");
        }

        return new TestResultDto
        {
            StartTime = test.StartTime,
            EndTime = test.EndTime,
            Score = test.TotalScore,
            TotalQuestions = test.ExamTemplate.NumberOfQuestions
        };
    }

    public async Task<StudentTest> CreateAsync(int templateId, string studentId)
    {
        var template = await _unitOfWork.ExamTemplates.GetByIdAsync(templateId);
        if (template == null)
        {
            throw new InvalidOperationException("Шаблон тесту не знайдено.");
        }

        var studentTest = new StudentTest
        {
            TemplateId = templateId,
            StudentId = studentId,
            StartTime = DateTime.UtcNow,
            CurrentQuestionIndex = 0
        };

        await _unitOfWork.StudentTests.AddAsync(studentTest);
        await _unitOfWork.SaveChangesAsync();
        return studentTest;
    }

    public async Task<TakeTestDto> GetCurrentQuestionAsync(int testId)
    {
        var test = await _unitOfWork.StudentTests.GetWithResponsesAsync(testId);
        if (test == null)
        {
            throw new InvalidOperationException("Тест не знайдено.");
        }

        if (test.EndTime.HasValue)
        {
            throw new InvalidOperationException("Тест уже завершений.");
        }

        var template = await _unitOfWork.ExamTemplates.GetWithQuestionsAsync(test.TemplateId);
        if (test.CurrentQuestionIndex >= template.Questions.Count)
        {
            return new TakeTestDto
            {
                TestId = test.Id,
                CurrentQuestionIndex = test.CurrentQuestionIndex,
                TotalQuestions = template.Questions.Count,
                CurrentQuestion = null
            };
        }

        var remainingTime = (int)(template.TimeLimit * 60 - (DateTime.UtcNow - test.StartTime).TotalSeconds);
        if (remainingTime <= 0)
        {
            await CompleteTestAsync(testId);
            throw new InvalidOperationException("Час на тест вичерпано.");
        }

        var currentQuestion = template.Questions.OrderBy(q => q.Id).Skip(test.CurrentQuestionIndex).First();
        return new TakeTestDto
        {
            TestId = test.Id,
            CurrentQuestionIndex = test.CurrentQuestionIndex,
            TotalQuestions = template.Questions.Count,
            RemainingTimeSeconds = remainingTime,
            CurrentQuestion = new QuestionTakeDto
            {
                QuestionId = currentQuestion.Id,
                Text = currentQuestion.Text,
                Answers = currentQuestion.Answers.Select(a => new AnswerTakeDto
                {
                    AnswerId = a.Id,
                    Text = a.Text
                }).ToList()
            }
        };
    }

    public async Task SaveResponsesAsync(StudentResponseDto responseDto)
    {
        var test = await _unitOfWork.StudentTests.GetByIdAsync(responseDto.TestId);
        if (test == null || test.EndTime.HasValue)
        {
            throw new InvalidOperationException("Тест не знайдено або вже завершений.");
        }

        var question = await _unitOfWork.Questions.GetWithAnswersAsync(responseDto.QuestionId);
        var selectedAnswers = question.Answers.Where(a => responseDto.SelectedAnswerIds.Contains(a.Id)).ToList();

        var studentResponse = new StudentResponse
        {
            TestId = responseDto.TestId,
            QuestionId = responseDto.QuestionId,
            SelectedAnswers = selectedAnswers
        };

        await _unitOfWork.StudentResponses.AddAsync(studentResponse);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MoveToNextQuestionAsync(int testId)
    {
        var test = await _unitOfWork.StudentTests.GetByIdAsync(testId);
        if (test == null || test.EndTime.HasValue)
        {
            throw new InvalidOperationException("Тест не знайдено або вже завершений.");
        }

        test.CurrentQuestionIndex++;
        _unitOfWork.StudentTests.Update(test);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteTestAsync(int testId)
    {
        var test = await _unitOfWork.StudentTests.GetByIdAsync(testId);
        if (test == null || test.EndTime.HasValue)
        {
            throw new InvalidOperationException("Тест не знайдено або вже завершений.");
        }

        test.EndTime = DateTime.UtcNow;

        test.TotalScore = await CalculateScoreAsync(testId);

        _unitOfWork.StudentTests.Update(test);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<int> CalculateScoreAsync(int testId)
    {
        var test = await _unitOfWork.StudentTests.GetWithResponsesAsync(testId);
        if (test == null)
        {
            throw new InvalidOperationException("Тест не знайдено.");
        }

        // Отримуємо всі відповіді студента для цього тесту
        var responses = test.StudentResponses.ToList();

        // Отримуємо всі питання, на які студент відповідав
        var questionIds = responses.Select(r => r.QuestionId).Distinct().ToList();
        var questions = await _unitOfWork.Questions
            .GetAll()
            .Where(q => questionIds.Contains(q.Id))
            .Include(q => q.Answers)
            .ToListAsync();

        int score = 0;

        foreach (var response in responses)
        {
            var question = questions.First(q => q.Id == response.QuestionId);
            var correctAnswers = question.Answers.Where(a => a.IsCorrect).ToList();
            var selectedAnswers = response.SelectedAnswers.ToList();

            bool isCorrect;
            if (correctAnswers.Count > 1)
            {
                var selectedAnswerIds = selectedAnswers.Select(a => a.Id).ToList();
                var correctAnswerIds = correctAnswers.Select(a => a.Id).ToList();
                isCorrect = selectedAnswerIds.Count == correctAnswerIds.Count &&
                           selectedAnswerIds.All(id => correctAnswerIds.Contains(id));
            }
            else
            {
                isCorrect = selectedAnswers.Count == 1 && selectedAnswers.First().IsCorrect;
            }

            if (isCorrect)
            {
                score++;
            }
        }

        return score;
    }
}
