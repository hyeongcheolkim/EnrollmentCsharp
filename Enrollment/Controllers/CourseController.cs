using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Enrollment.Data;
using Enrollment.Dtos;
using Enrollment.Dtos.Requests;
using Enrollment.Dtos.Response;
using Enrollment.Extensions;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers;

[ApiController]
[Route("/api/course")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CourseController(ICourseService courseService, ApplicationDbContext context, IMapper mapper)
    {
        _courseService = courseService;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("open")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult> OpenCourse([FromBody] CourseOpenRequest request)
    {
        var professorId = User.GetUserId();

        await _courseService.OpenAsync(request, professorId);
        return Created();
    }

    [HttpPost("close")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult> Close([FromQuery] long courseId)
    {
        var professorId = User.GetUserId();

        await _courseService.CloseAsync(courseId, professorId);
        return Ok();
    }

    [HttpPost("prohibit-dept")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult> ProhibitDepartment([FromQuery] long courseId, [FromQuery] long departmentId)
    {
        var professorId = User.GetUserId();

        await _courseService.AddProhibitedDepartmentAsync(courseId, departmentId, professorId);
        return Ok();
    }

    [HttpPost("/api/course/list")]
    [Authorize(Roles = UserRole.AnyLogin)]
    public async Task<ActionResult<PageableResponse<CourseResponse>>> CourseList()
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .Where(c => c.Activated && c.Subject.Activated)
            .ProjectTo<CourseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var response = new PageableResponse<CourseResponse>(courses);
        return Ok(response);
    }

    [HttpPost("/api/course/professor/list")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult<PageableResponse<CourseResponse>>> ProfessorCourseList()
    {
        var professorId = User.GetUserId();

        var courses = await _context.Courses
            .AsNoTracking()
            .Where(c => c.ProfessorId == professorId && c.Activated)
            .ProjectTo<CourseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var response = new PageableResponse<CourseResponse>(courses);
        return Ok(response);
    }
}