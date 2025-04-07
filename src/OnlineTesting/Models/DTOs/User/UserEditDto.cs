using System.ComponentModel.DataAnnotations;

namespace OnlineTesting.Models.DTOs.User;

public class UserEditDto
{
    public string Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string Surname { get; set; }

    public string MiddleName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Role { get; set; }
}
