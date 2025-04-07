using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Models;
using OnlineTesting.Models.DTOs.User;

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
}
