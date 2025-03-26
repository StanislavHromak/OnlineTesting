using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using OnlineTesting.Models.DTOs.ExamTemplate;

namespace OnlineTesting.Controllers;

public class ExamTemplateController : Controller
{
    private readonly IExamTemplateService _examTemplateService;
    private readonly IDisciplineService _disciplineService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExamTemplateController(
        IExamTemplateService examTemplateService,
        IDisciplineService disciplineService,
        UserManager<ApplicationUser> userManager)
    {
        _examTemplateService = examTemplateService;
        _disciplineService = disciplineService;
        _userManager = userManager;
    }

    // GET: ExamTemplate
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");
        return View();
    }

    // GET: ExamTemplate/GetTemplatesByDiscipline
    [Authorize]
    public async Task<IActionResult> GetTemplatesByDiscipline(int disciplineId)
    {
        var templates = await _examTemplateService.GetByDisciplineIdAsync(disciplineId);
        return PartialView("_TemplatesList", templates);
    }

    // GET: ExamTemplate/Create
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create()
    {
        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");
        return View(new ExamTemplateDto());
    }

    // POST: ExamTemplate/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create(ExamTemplateDto templateDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Отримуємо ID поточного користувача (викладача)
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                templateDto.TeacherId = user.Id;

                // Створюємо шаблон із випадковими питаннями
                await _examTemplateService.CreateWithRandomQuestionsAsync(templateDto);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");
        return View(templateDto);
    }

    // GET: ExamTemplate/Edit/5
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id)
    {
        var examTemplate = await _examTemplateService.GetByIdAsync(id);
        if (examTemplate == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (examTemplate.TeacherId != user.Id)
        {
            return Forbid();
        }

        var examTemplateEditDto = new ExamTemplateEditDto
        {
            Id = examTemplate.Id,
            Name = examTemplate.Name,
            Duration = examTemplate.TimeLimit
        };
        return View(examTemplateEditDto);
    }

    // POST: ExamTemplate/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id, ExamTemplateEditDto examTemplateEditDto)
    {
        if (id != examTemplateEditDto.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var examTemplate = await _examTemplateService.GetByIdAsync(id);
                if (examTemplate == null)
                {
                    return NotFound();
                }

                examTemplate.Name = examTemplateEditDto.Name;
                examTemplate.TimeLimit = examTemplateEditDto.Duration;

                await _examTemplateService.UpdateAsync(examTemplate);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Не вдалося зберегти зміни. Спробуйте ще раз.");
            }
        }
        return View(examTemplateEditDto);
    }

    // GET: ExamTemplate/Delete/5
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var template = await _examTemplateService.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        // Перевіряємо, чи належить шаблон поточному викладачу
        var user = await _userManager.GetUserAsync(User);
        if (template.TeacherId != user.Id)
        {
            return Forbid();
        }

        var templateDto = new ExamTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            DisciplineId = template.DisciplineId,
            QuestionCount = template.NumberOfQuestions,
            DurationMinutes = template.TimeLimit,
            TeacherId = template.TeacherId
        };

        return View(templateDto);
    }

    // POST: ExamTemplate/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var template = await _examTemplateService.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        // Перевіряємо, чи належить шаблон поточному викладачу
        var user = await _userManager.GetUserAsync(User);
        if (template.TeacherId != user.Id)
        {
            return Forbid();
        }

        await _examTemplateService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}