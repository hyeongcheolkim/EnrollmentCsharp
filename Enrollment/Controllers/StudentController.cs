using System.Security.Claims;
using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos;
using Enrollment.Dtos.Requests;
using Enrollment.Dtos.Response;
using Enrollment.Extensions;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers;

[ApiController]
[Route("/api/student")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IAuthHelper _authHelper;

    public StudentController(IStudentService studentService, IMapper mapper, ApplicationDbContext context,
        IAuthHelper authHelper)
    {
        _studentService = studentService;
        _mapper = mapper;
        _context = context;
        _authHelper = authHelper;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] StudentRegisterRequest request)
    {
        await _studentService.RegisterAsync(request);
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromQuery] string loginId, [FromQuery] string pw)
    {
        var student = await _studentService.LoginAsync(loginId, pw);

        if (student == null)
        {
            return Unauthorized(new { message = "로그인 실패: ID 또는 비밀번호가 올바르지 않습니다." });
        }

        var principal = _authHelper.CreateClaimsPrincipal(student.Id, student.MemberInfo.Name, UserRole.Student);
        await HttpContext.SignInAsync("Cookies", principal);

        var loginResponse = _mapper.Map<LoginResponse>(student);
        return Ok(loginResponse);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Ok();
    }

    [HttpPost("inactive")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult> Inactive()
    {
        var studentId = User.GetUserId();

        await _studentService.InactiveAsync(studentId);
        return Ok();
    }

    [HttpPost("basket")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult<PageableResponse<BasketResponse>>> GetBaskets()
    {
        var studentId = User.GetUserId();

        var baskets = await _context.Baskets
            .AsNoTracking()
            .Include(b => b.Student.MemberInfo)
            .Include(b => b.Course.Subject)
            .Where(b => b.StudentId == studentId)
            .ToListAsync();

        var basketResponses = _mapper.Map<List<BasketResponse>>(baskets);

        var response = new PageableResponse<BasketResponse>(basketResponses);
        return Ok(response);
    }
}