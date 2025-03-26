namespace OnlineTesting.Models;

public class StudentResponse
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public int QuestionId { get; set; }

    public StudentTest StudentTest { get; set; }
    public Question Question { get; set; }
    public ICollection<Answer> SelectedAnswers { get; set; }
}
