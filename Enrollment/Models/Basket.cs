using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Basket
{
    public long Id { get; set; }

    [Required]
    public Student Student { get; set; }
    public long StudentId { get; set; }
    
    [Required]
    public Course Course { get; set; }
    public long CourseId { get; set; }
}