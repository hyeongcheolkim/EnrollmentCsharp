using Enrollment.Models;

namespace Enrollment.Dtos.Response;

public class CourseResponse
{
    public long CourseId { get; set; }
    public long SubjectId { get; set; }
    public string SubjectName { get; set; }
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? Capacity { get; set; }
    public int? StudentYear { get; set; }
    public int? OpenYear { get; set; }
    public int? OpenSemester { get; set; }
    public int? Division { get; set; }
    public int? SubjectCode { get; set; }
    public long ProfessorId { get; set; }
    public string ProfessorName { get; set; }
    public CourseTime CourseTime { get; set; }
    public List<DepartmentResponse> ProhibitedDepartments { get; set; }
}