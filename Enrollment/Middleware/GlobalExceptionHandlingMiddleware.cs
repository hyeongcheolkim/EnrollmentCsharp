using System.Net;
using System.Text.Json;
using Enrollment.Dtos;

namespace Enrollment.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        string exceptionType = "ServerError";
        string exceptionName = exception.GetType().Name;

        var responseDto = new ApiExceptionDto
        {
            ExceptionType = exceptionType,
            ExceptionName = exceptionName,
            Message = exception.Message
        };

        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);


        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(responseDto, options));
    }
}