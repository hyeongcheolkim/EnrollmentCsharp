using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Enrollment.Data;
using Enrollment.Dtos;
using Enrollment.Dtos.Response;
using Enrollment.Extensions;
using Enrollment.Models;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers;

[ApiController]
[Route("api/enroll")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EnrollmentController(IEnrollmentService enrollmentService, ApplicationDbContext context, IMapper mapper)
    {
        _enrollmentService = enrollmentService;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("enroll")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult> Enroll([FromQuery] long courseId)
    {
        var studentId = User.GetUserId();

        await _enrollmentService.EnrollAsync(studentId, courseId);
        return Ok();
    }

    [HttpPost("enroll/baskets")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult<Dictionary<long, bool>>> EnrollBaskets([FromQuery] List<long> basketIds)
    {
        var studentId = User.GetUserId();


        var results = await _enrollmentService.EnrollBasketsAsync(studentId, basketIds);

        return Ok(results);
    }

    [HttpPost("drop")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult> Drop([FromQuery] long enrollmentId)
    {
        var studentId = User.GetUserId();
        
        await _enrollmentService.DropAsync(studentId, enrollmentId);
        return Ok();
    }

    [HttpPost("grade")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult> Grade([FromQuery] long enrollmentId, [FromQuery] ScoreType scoreType)
    {
        var professorId = User.GetUserId();

        await _enrollmentService.GradeAsync(professorId, enrollmentId, scoreType);
        return Ok();
    }

    [HttpPost("not-semester/student")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult<PageableResponse<NotSemesterEnrollResponse>>> GetNotSemesterEnrollments()
    {
        var studentId = User.GetUserId();

        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId && !e.OnSemester)
            .ProjectTo<NotSemesterEnrollResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var response = new PageableResponse<NotSemesterEnrollResponse>(enrollments);
        return Ok(response);
    }

    [HttpPost("on-semester/student")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult<PageableResponse<OnSemesterEnrollResponse>>> GetOnSemesterEnrollments()
    {
        var studentId = User.GetUserId();


        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId && e.OnSemester)
            .ProjectTo<OnSemesterEnrollResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var response = new PageableResponse<OnSemesterEnrollResponse>(enrollments);
        return Ok(response);
    }

    [HttpPost("on-semester/professor")]
    [Authorize(Roles = UserRole.Professor)]
    public async Task<ActionResult<PageableResponse<OnSemesterEnrollResponse>>> GetProfessorOnSemesterEnrollments()
    {
        var professorId = User.GetUserId();

        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.Course.ProfessorId == professorId && e.OnSemester)
            .ProjectTo<OnSemesterEnrollResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var response = new PageableResponse<OnSemesterEnrollResponse>(enrollments);
        return Ok(response);
    }
}