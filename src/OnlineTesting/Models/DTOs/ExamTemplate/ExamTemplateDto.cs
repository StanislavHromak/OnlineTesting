using System.ComponentModel.DataAnnotations;

namespace OnlineTesting.Models.DTOs.ExamTemplate;

public class ExamTemplateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Назва є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Оберіть дисципліну.")]
    public int DisciplineId { get; set; }

    [Required(ErrorMessage = "Вкажіть кількість питань.")]
    [Range(1, 100, ErrorMessage = "Кількість питань має бути від 1 до 100.")]
    public int QuestionCount { get; set; }

    [Required(ErrorMessage = "Вкажіть тривалість тесту.")]
    [Range(1, 240, ErrorMessage = "Тривалість тесту має бути від 1 до 240 хвилин.")]
    public int DurationMinutes { get; set; }

    public string? TeacherId { get; set; }
}