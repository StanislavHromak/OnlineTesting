namespace OnlineTesting.Models;

public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Question Question { get; set; }
    public ICollection<StudentResponse> StudentResponses { get; set; }
}
