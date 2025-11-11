using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Enrollment.Models;

public class Enroll
{
    public long Id { get; set; }
    
    [Required]
    public Course Course { get; set; }
    public long CourseId { get; set; }
    
    [Required]
    public Student Student { get; set; }
    public long StudentId { get; set; }

    public bool OnSemester { get; set; } = true;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScoreType? ScoreType { get; set; } 
}