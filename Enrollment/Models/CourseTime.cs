using System.Text.Json.Serialization;

namespace Enrollment.Models;

public class CourseTime
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Day Day { get; set; }

    public int? StartHour { get; set; } 
    public int? EndHour { get; set; }
}