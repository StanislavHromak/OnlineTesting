namespace OnlineTesting.Models.DTOs.StudentTest;

public class StudentTestResultDto
{
    public int TestId { get; set; }
    public string TemplateName { get; set; }
    public string StudentName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
}
