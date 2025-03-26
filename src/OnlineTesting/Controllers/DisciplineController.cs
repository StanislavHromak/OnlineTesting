using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Controllers;

public class DisciplineController : Controller
{
    private readonly IDisciplineService _disciplineService;

    public DisciplineController(IDisciplineService disciplineService)
    {
        _disciplineService = disciplineService;
    }

    // GET: Discipline
    //[Authorize]
    public async Task<IActionResult> Index()
    {
        var disciplines = await _disciplineService.GetAllAsync();
        return View(disciplines);
    }

    // GET: Discipline/Create
    //[Authorize(Roles = "Teacher")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Discipline/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create(Discipline discipline)
    {
        if (ModelState.IsValid)
        {
            await _disciplineService.CreateAsync(discipline);
            return RedirectToAction(nameof(Index));
        }
        return View(discipline);
    }

    // GET: Discipline/Edit/5
    //[Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id)
    {
        var discipline = await _disciplineService.GetByIdAsync(id);
        if (discipline == null)
        {
            return NotFound();
        }
        return View(discipline);
    }

    // POST: Discipline/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Edit(int id, Discipline discipline)
    {
        if (id != discipline.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _disciplineService.UpdateAsync(discipline);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again.");
            }
        }
        return View(discipline);
    }

    // GET: Discipline/Delete/5
    //[Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var discipline = await _disciplineService.GetByIdAsync(id);
        if (discipline == null)
        {
            return NotFound();
        }
        return View(discipline);
    }

    // POST: Discipline/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    //[Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _disciplineService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
