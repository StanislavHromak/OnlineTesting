namespace OnlineTesting.Models;

public class StudentResponse
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }

    public StudentTest StudentTest { get; set; }
    public Question Question { get; set; }
    public Answer Answer { get; set; }
}
