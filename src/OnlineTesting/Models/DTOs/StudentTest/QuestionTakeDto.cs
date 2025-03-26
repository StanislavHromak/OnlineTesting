namespace OnlineTesting.Models.DTOs.StudentTest;

public class QuestionTakeDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public List<AnswerTakeDto> Answers { get; set; }
}
