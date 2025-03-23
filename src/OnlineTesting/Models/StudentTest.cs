namespace OnlineTesting.Models;

public class StudentTest
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string StudentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int TotalScore { get; set; }

    public ExamTemplate ExamTemplate { get; set; }
    public ApplicationUser Student { get; set; }
    public ICollection<StudentResponse> StudentResponses { get; set; }
}
