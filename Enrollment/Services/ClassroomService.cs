using AutoMapper;
using Enrollment.Data;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class ClassroomService : IClassroomService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClassroomService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Classroom> CreateAsync(string name, int code)
    {
        if (await _context.Classrooms.AnyAsync(c => c.Code == code))
        {
            throw new Exception("교실 코드가 중복됩니다"); 
        }
        
        var classroom = new Classroom
        {
            Name = name,
            Code = code,
            Activated = true
        };

        _context.Classrooms.Add(classroom);
        await _context.SaveChangesAsync();
        return classroom;
    }
    
    public async Task<bool> InactiveAsync(long classroomId)
    {
        var classroom = await _context.Classrooms.FindAsync(classroomId);
        if (classroom == null)
        {
            throw new Exception("존재하지 않는 교실입니다.");
        }

        classroom.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}