using Microsoft.AspNetCore.Identity;

namespace OnlineTesting.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }

    public ICollection<ExamTemplate> ExamTemplates { get; set; }
    public ICollection<StudentTest> StudentTests { get; set; }
}