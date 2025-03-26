using System.ComponentModel.DataAnnotations;

namespace OnlineTesting.Models.DTOs.ExamTemplate;

public class ExamTemplateEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Назва є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Поле Тривалість є обов'язковим.")]
    [Range(1, 300, ErrorMessage = "Тривалість має бути від 1 до 300 хвилин.")]
    public int Duration { get; set; }
}
