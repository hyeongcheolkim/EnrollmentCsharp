using System.ComponentModel.DataAnnotations;
using Enrollment.Models;

namespace Enrollment.Dtos.Requests;

public class CourseOpenRequest
{
    [Required]
    public long SubjectId { get; set; }

    [Required]
    public long DepartmentId { get; set; }

    [Required]
    public int? Capacity { get; set; }

    [Required]
    [Range(1, 4, ErrorMessage = "1~4 숫자만 입력 가능합니다")]
    public int? StudentYear { get; set; }

    [Required]
    public int? OpenYear { get; set; }

    [Required]
    [Range(1, 2, ErrorMessage = "1 또는 2여야 합니다")]
    public int? OpenSemester { get; set; }

    [Required]
    public int? Division { get; set; }

    public long? ClassroomId { get; set; } 

    public List<long>? ProhibitedDepartmentIds { get; set; }
    
    [Required]
    public CourseTime CourseTime { get; set; }
}