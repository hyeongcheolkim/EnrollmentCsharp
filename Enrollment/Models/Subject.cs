using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Enrollment.Models;

public class Subject
{
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int? Credit { get; set; }

    public int? Code { get; set; }

    public bool Activated { get; set; } = true;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public SubjectType Type { get; set; }
}