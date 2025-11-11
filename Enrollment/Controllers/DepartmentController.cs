using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Dtos.Response;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers;

[ApiController]
[Route("api/department")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentController(IDepartmentService departmentService, ApplicationDbContext context, IMapper mapper)
    {
        _departmentService = departmentService;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("create")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
    {
        await _departmentService.CreateAsync(request);
        return Created();
    }

    [HttpPost("inactivate")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> InactivateDepartment([FromQuery] long departmentId)
    {
        await _departmentService.InactiveAsync(departmentId);
        return Ok();
    }

    [HttpPost("list")]
    public async Task<ActionResult<DepartmentResponse>> DepartmentList()
    {
        var departments = await _context.Departments
            .Where(d => d.Activated)
            .ToListAsync();

        var departmentResponses = _mapper.Map<List<DepartmentResponse>>(departments);

        var response = new DepartmentListResponse
        {
            Departments = departmentResponses
        };

        return Ok(response);
    }
}