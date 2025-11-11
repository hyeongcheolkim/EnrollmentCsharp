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
[Route("/api/subject")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SubjectController(ISubjectService subjectService, ApplicationDbContext context, IMapper mapper)
    {
        _subjectService = subjectService;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("make")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> MakeSubject([FromBody] SubjectMakeRequest request)
    {
        await _subjectService.CreateAsync(request);
        return Created();
    }

    [HttpPost("inactive")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult> InactiveSubject([FromQuery] long subjectId)
    {
        await _subjectService.InactiveAsync(subjectId);
        return Ok();
    }


    [HttpPost("list")]
    public async Task<ActionResult<SubjectListResponse>> SubjectList()
    {
        var subjects = await _context.Subjects
            .Where(s => s.Activated)
            .ToListAsync();

        var subjectResponses = _mapper.Map<List<SubjectResponse>>(subjects);

        var response = new SubjectListResponse
        {
            Subjects = subjectResponses
        };

        return Ok(response);
    }
}