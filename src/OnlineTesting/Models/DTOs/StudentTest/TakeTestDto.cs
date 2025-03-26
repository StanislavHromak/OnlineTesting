namespace OnlineTesting.Models.DTOs.StudentTest;

public class TakeTestDto
{
    public int TestId { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public int TotalQuestions { get; set; }
    public int RemainingTimeSeconds { get; set; }
    public QuestionTakeDto CurrentQuestion { get; set; }
}
