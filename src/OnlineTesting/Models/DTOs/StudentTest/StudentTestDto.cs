namespace OnlineTesting.Models.DTOs.StudentTest;

public class StudentTestDto
{
    public int TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string DisciplineName { get; set; }
    public int Duration { get; set; }
    public int QuestionCount { get; set; }
    public bool HasTaken { get; set; }
    public int? Score { get; set; }
}
