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
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;

    public AdminController(IAdminService adminService, IMapper mapper, IAuthHelper authHelper)
    {
        _adminService = adminService;
        _mapper = mapper;
        _authHelper = authHelper;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] AdminRegisterRequest request)
    {
        await _adminService.RegisterAsync(request);
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromQuery] string loginId, [FromQuery] string pw)
    {
        var admin = await _adminService.LoginAsync(loginId, pw);

        if (admin == null)
        {
            return Unauthorized(new ApiExceptionDto { Message = "로그인 실패: ID 또는 비밀번호가 올바르지 않습니다." });
        }


        var principal = _authHelper.CreateClaimsPrincipal(admin.Id, admin.MemberInfo.Name, UserRole.Admin);
        await HttpContext.SignInAsync("Cookies", principal);


        var loginResponse = _mapper.Map<LoginResponse>(admin);
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
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> Inactive()
    {
        var adminId = User.GetUserId();

        await _adminService.InactiveAsync(adminId);
        return Ok();
    }
}