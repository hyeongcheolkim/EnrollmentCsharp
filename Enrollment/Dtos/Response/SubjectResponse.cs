using System.Text.Json.Serialization;
using Enrollment.Models;

namespace Enrollment.Dtos.Response;

public class SubjectResponse
{
    public long SubjectId { get; set; }
    public string SubjectName { get; set; }
    public int? Credit { get; set; }
    public int? Code { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SubjectType Type { get; set; }
}