using System.ComponentModel.DataAnnotations;

namespace Enrollment.Dtos.Requests;

public class CreateDepartmentRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int? Code { get; set; }
}