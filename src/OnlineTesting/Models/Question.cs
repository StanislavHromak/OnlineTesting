namespace OnlineTesting.Models;

public class Question
{
    public int Id { get; set; }
    public int DisciplineId { get; set; }
    public string Text { get; set; }
    public int NumberOfAnswers { get; set; }

    public Discipline Discipline { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<StudentResponse> StudentResponses { get; set; }
    public ICollection<ExamTemplate> ExamTemplates { get; set; }
}
