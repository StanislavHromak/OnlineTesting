using Microsoft.AspNetCore.Identity;
using OnlineTesting.Models;

namespace OnlineTesting.Data;

public class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Teacher", "Student", "Dean" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }

        var deanEmail = "dean@example.com";
        var dean = await userManager.FindByEmailAsync(deanEmail);
        if (dean == null)
        {
            dean = new ApplicationUser
            {
                UserName = deanEmail,
                Name = "Петро",
                MiddleName = "Петренко",
                Surname = "Петренко",
                Email = deanEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(dean, "DeanPassword123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(dean, "Teacher");
            }
            else
            {
                throw new Exception($"Не вдалося створити декана: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
