using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Professor
{
    public long Id { get; set; }

    [Required]
    public MemberInfo MemberInfo { get; set; }
}
    