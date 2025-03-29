namespace OnlineTesting.Models.DTOs.StudentTest;

public class TestResultDto
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
}