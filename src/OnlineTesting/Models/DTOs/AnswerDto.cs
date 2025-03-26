using System.ComponentModel.DataAnnotations;

namespace OnlineTesting.Models.DTOs;
public class AnswerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Текст відповіді є обов'язковим.")]
    [StringLength(200, ErrorMessage = "Текст відповіді не може перевищувати 200 символів.")]
    public string Text { get; set; }

    public bool IsCorrect { get; set; }
}
