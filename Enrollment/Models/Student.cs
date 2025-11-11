using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Student
{
    public long Id { get; set; }

    [Required]
    public MemberInfo MemberInfo { get; set; } 
    
    [Required]
    public Department Department { get; set; }
    public long DepartmentId { get; set; }
    public ICollection<Enroll> Enrollments { get; set; } = new List<Enroll>();
    public ICollection<Basket> Baskets { get; set; } = new List<Basket>();
}