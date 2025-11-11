using System.Security.Claims;
using System.Text.Json; 
using Microsoft.AspNetCore.Authentication.Cookies; 
using Enrollment.Data;
using Enrollment.Dtos;
using Enrollment.Middleware;
using Enrollment.Services;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "JSESSIONID"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
        options.SlidingExpiration = true;


        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401; 
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogout = context =>
        {
            context.Response.StatusCode = 200; 
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IAuthHelper, AuthHelper>(); 

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var apiExceptionDto = new ApiExceptionDto();
            
            var (key, value) = context.ModelState.FirstOrDefault(ms => ms.Value.Errors.Any());
            var firstError = value?.Errors.FirstOrDefault();

            if (firstError == null)
            {
                apiExceptionDto.ExceptionType = "Validation Error";
                apiExceptionDto.ExceptionName = "UnknownValidationException";
                apiExceptionDto.Message = "알 수 없는 유효성 검사 오류가 발생했습니다.";
            }

            else if (firstError.Exception is JsonException jsonEx)
            {
                apiExceptionDto.ExceptionType = "Model Binding Error";
                apiExceptionDto.ExceptionName = jsonEx.GetType().Name;
                

                var fieldName = key.StartsWith("$.") ? key.Substring(2) : key;
                apiExceptionDto.Message = $"'{fieldName}' 필드의 형식이 올바르지 않습니다. (입력값 확인 필요)";
            }
         
            else
            {
                apiExceptionDto.ExceptionType = "Validation Error";
               
                apiExceptionDto.ExceptionName = "ValidationException"; 
                
               
                if (key.Equals("request", StringComparison.OrdinalIgnoreCase) && firstError.ErrorMessage.Contains("required"))
                {
                    apiExceptionDto.Message = "요청 본문(body)이 비어있거나 형식이 잘못되었습니다.";
                    apiExceptionDto.ExceptionName = "NullBodyException";
                }
                else
                {

                    apiExceptionDto.Message = $"'{key}': {firstError.ErrorMessage}";
                }
            }

            return new BadRequestObjectResult(apiExceptionDto)
            {
                StatusCode = 400
            };
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); 


app.UseAuthentication(); 
app.UseAuthorization(); 

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();