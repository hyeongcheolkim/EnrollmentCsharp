using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class ProfessorService : IProfessorService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public ProfessorService(ApplicationDbContext context, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }
    
    public async Task<Professor?> LoginAsync(string loginId, string pw)
    {
        var professor = await _context.Professors
            .FirstOrDefaultAsync(p => p.MemberInfo.LoginId == loginId && p.MemberInfo.Activated);

        if (professor == null)
        {
            return null;
        }

        if (!_passwordHasher.VerifyPassword(pw, professor.MemberInfo.Pw))
        {
            return null;
        }

        return professor;
    }
    
    public async Task<Professor> RegisterAsync(ProfessorRegisterRequest request)
    {
        if (await _context.Professors.AnyAsync(p => p.MemberInfo.LoginId == request.LoginId && p.MemberInfo.Activated))
        {
            throw new Exception("이미 존재하는 ID입니다."); 
        }

        var professor = _mapper.Map<Professor>(request);
        professor.MemberInfo.Pw = _passwordHasher.HashPassword(request.Pw);

        _context.Professors.Add(professor);
        await _context.SaveChangesAsync();

        return professor;
    }
    
    public async Task<bool> InactiveAsync(long professorId)
    {
        var professor = await _context.Professors.FindAsync(professorId);

        if (professor == null)
        {
            throw new Exception("존재하지 않는 교수입니다."); 
        }

        professor.MemberInfo.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}