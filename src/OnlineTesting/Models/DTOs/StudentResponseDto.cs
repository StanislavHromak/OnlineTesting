namespace OnlineTesting.Models.DTOs;

public class StudentResponseDto
{
    public int TestId { get; set; }
    public int QuestionId { get; set; }
    public List<int> SelectedAnswerIds { get; set; }
}
