namespace OnlineTesting.Models;

public class ExamTemplate
{
    public int Id { get; set; }
    public int DisciplineId { get; set; }
    public string TeacherId { get; set; }
    public int NumberOfQuestions { get; set; }
    public int TimeLimit { get; set; }
    public DateTime CreatedAt { get; set; }

    public Discipline Discipline { get; set; }
    public ApplicationUser Teacher { get; set; }
    public ICollection<StudentTest> StudentTests { get; set; }
    public ICollection<Question> Questions { get; set; }
}
