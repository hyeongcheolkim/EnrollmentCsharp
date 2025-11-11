using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class SubjectService : ISubjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SubjectService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<Subject> CreateAsync(SubjectMakeRequest request)
    {

        if (await _context.Subjects.AnyAsync(s => s.Code == request.Code && s.Activated))
        {
            throw new Exception("과목코드가 중복됩니다");
        }

        var subject = _mapper.Map<Subject>(request);

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }
    
    public async Task<bool> InactiveAsync(long subjectId)
    {
        var subject = await _context.Subjects.FindAsync(subjectId);
        if (subject == null)
        {
            throw new Exception("존재하지 않는 과목입니다.");
        }

        subject.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}