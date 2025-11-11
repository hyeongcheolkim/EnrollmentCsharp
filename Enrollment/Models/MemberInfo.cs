using System.ComponentModel.DataAnnotations;

namespace Enrollment.Models;

public class MemberInfo
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string LoginId { get; set; }

    [Required]
    public string Pw { get; set; }

    public bool Activated { get; set; } = true;
}