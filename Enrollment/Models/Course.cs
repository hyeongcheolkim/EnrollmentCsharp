using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Course
{
    public long Id { get; set; }
    
    [Required]
    public Subject Subject { get; set; }
    public long SubjectId { get; set; }

    [Required]
    public Department Department { get; set; }
    public long DepartmentId { get; set; }

    public int? Capacity { get; set; }
    public int? StudentYear { get; set; }
    public int? OpenYear { get; set; }
    public int? OpenSemester { get; set; }
    public int? Division { get; set; }


    [Required]
    public Classroom Classroom { get; set; }
    public long ClassroomId { get; set; }

    [Required]
    public Professor Professor { get; set; }
    public long ProfessorId { get; set; }

    public bool Activated { get; set; } = true;


    [Required]
    public CourseTime CourseTime { get; set; }


    public ICollection<Department> ProhibitedDepartments { get; set; } = new List<Department>();


    public ICollection<Enroll> Enrollments { get; set; } = new List<Enroll>();
}