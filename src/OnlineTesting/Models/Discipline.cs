namespace OnlineTesting.Models;

public class Discipline
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<ExamTemplate> ExamTemplates { get; set; }
}
