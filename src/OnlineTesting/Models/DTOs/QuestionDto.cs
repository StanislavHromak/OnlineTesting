using System.ComponentModel.DataAnnotations;

namespace OnlineTesting.Models.DTOs;

public class QuestionDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Текст питання є обов'язковим.")]
    [StringLength(400, ErrorMessage = "Текст питання не може перевищувати 500 символів.")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Оберіть дисципліну.")]
    public int DisciplineId { get; set; }

    public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
}
