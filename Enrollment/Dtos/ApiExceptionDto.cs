namespace Enrollment.Dtos;

public class ApiExceptionDto
{
    public string ExceptionType { get; set; }
    public string ExceptionName { get; set; }
    public string Message { get; set; }
}