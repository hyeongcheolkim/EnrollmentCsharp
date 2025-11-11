using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class Admin
{
    public long Id { get; set; }

    [Required]
    public MemberInfo MemberInfo { get; set; } // Java의 @Embedded MemberInfo
}