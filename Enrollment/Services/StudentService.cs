using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public StudentService(ApplicationDbContext context, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }
    
    public async Task<Student?> LoginAsync(string loginId, string pw)
    {
        var student = await _context.Students
            .Include(s => s.Department) 
            .FirstOrDefaultAsync(s => s.MemberInfo.LoginId == loginId && s.MemberInfo.Activated);

        if (student == null)
        {
            return null;
        }

        if (!_passwordHasher.VerifyPassword(pw, student.MemberInfo.Pw))
        {
            return null;
        }

        return student;
    }


    public async Task<Student> RegisterAsync(StudentRegisterRequest request)
    {

        if (await _context.Students.AnyAsync(s => s.MemberInfo.LoginId == request.LoginId && s.MemberInfo.Activated))
        {
            throw new Exception("이미 존재하는 ID입니다."); 
        }
        
        var department = await _context.Departments.FindAsync(request.DepartmentId);
        if (department == null || !department.Activated)
        {
            throw new Exception("존재하지 않거나 비활성화된 학과입니다."); 
        }
        
        var student = _mapper.Map<Student>(request);
        
        student.MemberInfo.Pw = _passwordHasher.HashPassword(request.Pw);
        student.Department = department; 

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return student;
    }
    
    public async Task<bool> InactiveAsync(long studentId)
    {
        var student = await _context.Students.FindAsync(studentId);

        if (student == null)
        {
            throw new Exception("존재하지 않는 학생입니다."); 
        }

        student.MemberInfo.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}