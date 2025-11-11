using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Department
{
    public long Id { get; set; }
    [Required] 
    public string Name { get; set; }
    public int? Code { get; set; } 
    public bool Activated { get; set; } = true;
}