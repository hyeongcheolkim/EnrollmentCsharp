using System.Security.Claims;
using AutoMapper;
using Enrollment.Dtos;
using Enrollment.Dtos.Requests;
using Enrollment.Dtos.Response;
using Enrollment.Extensions;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enrollment.Controllers;

[ApiController]
[Route("api/professor")]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _professorService;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;

    public ProfessorController(IProfessorService professorService, IMapper mapper, IAuthHelper authHelper)
    {
        _professorService = professorService;
        _mapper = mapper;
        _authHelper = authHelper;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] ProfessorRegisterRequest request)
    {
        await _professorService.RegisterAsync(request);
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromQuery] string loginId, [FromQuery] string pw)
    {
        var professor = await _professorService.LoginAsync(loginId, pw);

        if (professor == null)
        {
            return Unauthorized(new ApiExceptionDto { Message = "로그인 실패: ID 또는 비밀번호가 올바르지 않습니다." });
        }


        var principal = _authHelper.CreateClaimsPrincipal(professor.Id, professor.MemberInfo.Name, UserRole.Professor);
        await HttpContext.SignInAsync("Cookies", principal);

        var loginResponse = _mapper.Map<LoginResponse>(professor);
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
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult> Inactive()
    {
        var professorId = User.GetUserId();

        await _professorService.InactiveAsync(professorId);
        return Ok();
    }
}