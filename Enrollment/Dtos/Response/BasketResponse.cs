namespace Enrollment.Dtos.Response;

public class BasketResponse
{
    public long BasketId { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; }
    public long CourseId { get; set; }
    public int? Capacity { get; set; }
    public long SubjectId { get; set; }
    public string SubjectName { get; set; }
    public int? Division { get; set; }
    public int? SubjectCode { get; set; }
}