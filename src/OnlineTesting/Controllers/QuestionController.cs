using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models.DTOs;
using OnlineTesting.Services.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly IDisciplineService _disciplineService;

    public QuestionController(IQuestionService questionService, IDisciplineService disciplineService)
    {
        _questionService = questionService;
        _disciplineService = disciplineService;
    }

    // GET: Question
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");
        return View();
    }

    // GET: Question/GetQuestionsByDiscipline
    [Authorize]
    public async Task<IActionResult> GetQuestionsByDiscipline(int disciplineId)
    {
        var questions = await _questionService.GetByDisciplineIdAsync(disciplineId);
        return PartialView("_QuestionsList", questions);
    }

    // GET: Question/Create
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create()
    {
        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");

        var questionDto = new QuestionDto
        {
            Answers = new List<AnswerDto>
            {
                new AnswerDto(),
                new AnswerDto(),
                new AnswerDto(),
                new AnswerDto()
            }
        };
        return View(questionDto);
    }

    // POST: Question/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create(QuestionDto questionDto)
    {
        if (ModelState.IsValid)
        {
            if (!questionDto.Answers.Any(a => a.IsCorrect))
            {
                ModelState.AddModelError("", "Потрібно позначити хоча б одну правильну відповідь.");
            }
            else
            {
                await _questionService.CreateWithAnswersAsync(questionDto);
                return RedirectToAction(nameof(Index));
            }
        }

        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name");
        return View(questionDto);
    }

    // GET: Question/Edit/5
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id)
    {
        var question = await _questionService.GetWithAnswersAsync(id);
        if (question == null)
        {
            return NotFound();
        }

        var questionDto = new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            DisciplineId = question.DisciplineId,
            Answers = question.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList()
        };

        while (questionDto.Answers.Count < 4)
        {
            questionDto.Answers.Add(new AnswerDto());
        }

        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name", questionDto.DisciplineId);
        ViewBag.IsUsedInCompletedTests = await _questionService.IsUsedInCompletedTestsAsync(id);
        return View(questionDto);
    }

    // POST: Question/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id, QuestionDto questionDto)
    {
        if (id != questionDto.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            if (!questionDto.Answers.Any(a => a.IsCorrect))
            {
                ModelState.AddModelError("", "Потрібно позначити хоча б одну правильну відповідь.");
            }
            else
            {
                try
                {
                    var question = await _questionService.GetWithAnswersAsync(id);
                    if (question == null)
                    {
                        return NotFound();
                    }

                    question.Text = questionDto.Text;
                    question.DisciplineId = questionDto.DisciplineId;

                    question.Answers.Clear();
                    question.Answers = questionDto.Answers
                        .Where(a => !string.IsNullOrEmpty(a.Text))
                        .Select(a => new Answer
                        {
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList();

                    await _questionService.UpdateAsync(question);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Не вдалося зберегти зміни. Спробуйте ще раз.");
                }
            }
        }

        var disciplines = await _disciplineService.GetAllAsync();
        ViewBag.Disciplines = new SelectList(disciplines, "Id", "Name", questionDto.DisciplineId);
        return View(questionDto);
    }

    // GET: Question/Delete/5
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var question = await _questionService.GetWithAnswersAsync(id);
        if (question == null)
        {
            return NotFound();
        }

        var questionDto = new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            DisciplineId = question.DisciplineId,
            Answers = question.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList()
        };

        return View(questionDto);
    }

    // POST: Question/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _questionService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
