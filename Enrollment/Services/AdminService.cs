using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context; 
    private readonly IPasswordHasher _passwordHasher; 
    private readonly IMapper _mapper;

    public AdminService(ApplicationDbContext context, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }
    
    public async Task<Admin?> LoginAsync(string loginId, string pw)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.MemberInfo.LoginId == loginId && a.MemberInfo.Activated);

        if (admin == null)
        {
            return null;
        }
        
        if (!_passwordHasher.VerifyPassword(pw, admin.MemberInfo.Pw))
        {
            return null;
        }

        return admin;
    }
    
    public async Task<Admin> RegisterAsync(AdminRegisterRequest request)
    {
        if (await _context.Admins.AnyAsync(a => a.MemberInfo.LoginId == request.LoginId && a.MemberInfo.Activated))
        {
            throw new Exception("이미 존재하는 ID입니다."); 
        }
        
        var admin = _mapper.Map<Admin>(request);
        
        admin.MemberInfo.Pw = _passwordHasher.HashPassword(request.Pw);
        
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        return admin;
    }
    
    public async Task<bool> InactiveAsync(long adminId)
    {
        var admin = await _context.Admins.FindAsync(adminId);

        if (admin == null)
        {
            throw new Exception("존재하지 않는 관리자입니다.");
        }

        admin.MemberInfo.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}