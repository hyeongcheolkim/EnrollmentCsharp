using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Response;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers;

[ApiController]
[Route("api/classroom")]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClassroomController(IClassroomService classroomService, ApplicationDbContext context, IMapper mapper)
    {
        _classroomService = classroomService;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("create")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> CreateClassroom([FromQuery] string name, [FromQuery] int code)
    {
        await _classroomService.CreateAsync(name, code);
        return Created();
    }

    [HttpPost("inactivate")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> InactivateClassroom([FromQuery] long classroomId)
    {
        await _classroomService.InactiveAsync(classroomId);
        return Ok();
    }

    [HttpPost("list")]
    public async Task<ActionResult<ClassroomListResponse>> ClassroomList()
    {
        var classrooms = await _context.Classrooms
            .Where(c => c.Activated)
            .ToListAsync();

        var classroomResponses = _mapper.Map<List<ClassroomResponse>>(classrooms);

        var response = new ClassroomListResponse
        {
            Classrooms = classroomResponses
        };

        return Ok(response);
    }
}