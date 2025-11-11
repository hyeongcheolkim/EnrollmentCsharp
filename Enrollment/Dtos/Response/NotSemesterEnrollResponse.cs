using System.Text.Json.Serialization;
using Enrollment.Models;

namespace Enrollment.Dtos.Response;

public class NotSemesterEnrollResponse
{
    public long EnrollmentId { get; set; } 
    public long SubjectId { get; set; }
    public int? SubjectCode { get; set; }
    public int? SubjectCredit { get; set; }
    public int? Division { get; set; }
    public int? CourseOpenYear { get; set; }
    public int? CourseOpenSemester { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScoreType? ScoreType { get; set; }
    public string SubjectName { get; set; }
}