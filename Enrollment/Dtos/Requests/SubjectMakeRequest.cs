using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Enrollment.Models;

namespace Enrollment.Dtos.Requests;

public class SubjectMakeRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int? Credit { get; set; }

    [Required]
    public int? Code { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public SubjectType Type { get; set; }
}