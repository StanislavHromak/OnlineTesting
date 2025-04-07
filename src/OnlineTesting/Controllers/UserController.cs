using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models;
using OnlineTesting.Models.DTOs.User;
using System.Security.Claims;

namespace OnlineTesting.Controllers;

[Authorize(Roles = "Dean,Teacher")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                FullName = $"{user.Surname} {user.Name} {user.MiddleName}",
                Email = user.Email,
                Role = roles.FirstOrDefault()
            });
        }

        return View(userDtos);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateDto model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser 
            {
                Name = model.FirstName,
                Surname = model.Surname,
                MiddleName = model.MiddleName,
                UserName = model.Email,
                Email = model.Email, 
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (User.IsInRole("Dean"))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else if (User.IsInRole("Teacher"))
                {
                    if (model.Role != "Student")
                    {
                        ModelState.AddModelError("", "Ви можете створювати лише учнів.");
                        return View(model);
                    }
                    await _userManager.AddToRoleAsync(user, "Student");
                }
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var model = new UserEditDto
        {
            Id = user.Id,
            FirstName = user.Name,
            Surname = user.Surname,
            MiddleName = user.MiddleName,
            Email = user.Email,
            Role = roles.FirstOrDefault()
        };

        return View(model);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UserEditDto model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (User.IsInRole("Teacher") && userRoles.Contains("Teacher"))
            {
                ModelState.AddModelError("", "Ви можете редагувати лише учнів.");
                return View(model);
            }

            user.Name = model.FirstName;
            user.Surname = model.Surname;
            user.MiddleName = model.MiddleName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Користувач успішно оновлений.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // GET: Users/Delete/5
    [Authorize(Roles = "Dean")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserDto
        {
            Id = user.Id,
            FullName = $"{user.Surname} {user.Name} {user.MiddleName}",
            Email = user.Email,
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
        };

        return View(model);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Dean")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (user.Id == currentUserId)
        {
            TempData["Error"] = "Ви не можете видалити самого себе.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "Користувач успішно видалений.";
        }
        else
        {
            TempData["Error"] = "Помилка при видаленні користувача.";
        }

        return RedirectToAction(nameof(Index));
    }
}
