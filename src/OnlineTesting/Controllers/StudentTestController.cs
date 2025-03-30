using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models.DTOs.StudentTest;
using OnlineTesting.Models.DTOs;
using OnlineTesting.Services.Abstractions;
using System.Security.Claims;

namespace OnlineTesting.Controllers;

public class StudentTestController : Controller
{
    private readonly IStudentTestService _studentTestService;
    private readonly IExamTemplateService _examTemplateService;

    public StudentTestController(IStudentTestService studentTestService, IExamTemplateService examTemplateService)
    {
        _studentTestService = studentTestService;
        _examTemplateService = examTemplateService;
    }

    // GET: StudentTest
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index()
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var templates = await _examTemplateService.GetAllWithDisciplineAsync(); // Отримуємо всі шаблони
        var studentTests = await _studentTestService.GetByStudentIdAsync(studentId);

        var studentTestDtos = templates.Select(t => new StudentTestDto
        {
            TemplateId = t.Id,
            TemplateName = t.Name,
            DisciplineName = t.Discipline?.Name,
            Duration = t.TimeLimit,
            QuestionCount = t.NumberOfQuestions,
            HasTaken = studentTests.Any(st => st.TemplateId == t.Id),
            Score = studentTests.FirstOrDefault(st => st.TemplateId == t.Id)?.TotalScore
        }).ToList();

        return View(studentTestDtos);
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> StudentResults(int templateId)
    {
        var studentTests = await _studentTestService.GetByTemplateIdAsync(templateId);

        var examTemplate = await _examTemplateService.GetByIdAsync(templateId);
        if (examTemplate == null)
        {
            return NotFound();
        }

        var studentTestDtos = studentTests.Select(t => new StudentTestResultDto
        {
            StudentName = t.Student.Name + " " + t.Student.Surname,
            StartTime = t.StartTime,
            EndTime = t.EndTime,
            Score = t.TotalScore,
            TotalQuestions = examTemplate.NumberOfQuestions
        }).ToList();

        return View(studentTestDtos);
    }

    // POST: StudentTest/StartTest
    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> StartTest(int templateId)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var studentTests = await _studentTestService.GetByStudentIdAsync(studentId);

        if (studentTests.Any(st => st.TemplateId == templateId))
        {
            return BadRequest("Ви вже проходили цей тест.");
        }

        var studentTest = await _studentTestService.CreateAsync(templateId, studentId);
        return RedirectToAction(nameof(TakeTest), new { testId = studentTest.Id });
    }

    // GET: StudentTest/TakeTest/5
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> TakeTest(int testId)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var test = await _studentTestService.GetByIdAsync(testId);

        if (test == null || test.StudentId != studentId)
        {
            return NotFound();
        }

        if (test.EndTime.HasValue)
        {
            return RedirectToAction(nameof(Result), new { testId });
        }

        try
        {
            var takeTestDto = await _studentTestService.GetCurrentQuestionAsync(testId);
            return View(takeTestDto);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Час на тест вичерпано.")
            {
                return RedirectToAction(nameof(Result), new { testId });
            }
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: StudentTest/SaveResponse
    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SaveResponse(StudentResponseDto responseDto)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var test = await _studentTestService.GetByIdAsync(responseDto.TestId);

        if (test == null || test.StudentId != studentId || test.EndTime.HasValue)
        {
            return Json(new { success = false, message = "Тест не знайдено або вже завершений." });
        }

        try
        {
            await _studentTestService.SaveResponsesAsync(responseDto);
            await _studentTestService.MoveToNextQuestionAsync(responseDto.TestId);

            var takeTestDto = await _studentTestService.GetCurrentQuestionAsync(responseDto.TestId);
            if (takeTestDto.CurrentQuestion == null)
            {
                await _studentTestService.CompleteTestAsync(responseDto.TestId);
                return Json(new { success = true, redirectTo = Url.Action(nameof(Result), new { testId = responseDto.TestId }) });
            }

            return Json(new { success = true });
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Час на тест вичерпано.")
            {
                return Json(new { success = false, redirectTo = Url.Action(nameof(Result), new { testId = responseDto.TestId }) });
            }
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: StudentTest/CompleteTest
    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> CompleteTest(int testId)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var test = await _studentTestService.GetByIdAsync(testId);

        if (test == null || test.StudentId != studentId || test.EndTime.HasValue)
        {
            return BadRequest("Тест не знайдено або вже завершений.");
        }

        await _studentTestService.CompleteTestAsync(testId);
        return RedirectToAction(nameof(Result), new { testId });
    }

    // GET: StudentTest/Result/5
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Result(int testId)
    {
        var result = await _studentTestService.GetTestResult(testId);
        return View(result);
    }
}
