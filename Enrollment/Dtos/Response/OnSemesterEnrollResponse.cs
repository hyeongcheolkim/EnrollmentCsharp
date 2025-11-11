namespace Enrollment.Dtos.Response;

public class OnSemesterEnrollResponse
{
    public long EnrollmentId { get; set; } 
    public long SubjectId { get; set; }
    public string SubjectName { get; set; }
    public int? SubjectCode { get; set; }
    public int? Division { get; set; }
    public string StudentName { get; set; }
    public string StudentDepartmentName { get; set; }
}