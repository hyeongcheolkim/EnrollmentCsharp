using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Department> CreateAsync(CreateDepartmentRequest request)
    {
        if (await _context.Departments.AnyAsync(d => d.Code == request.Code && d.Activated))
        {
            throw new Exception("학과 코드가 중복됩니다"); 
        }

        var department = _mapper.Map<Department>(request);

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }
    
    public async Task<bool> InactiveAsync(long departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
        {
            throw new Exception("존재하지 않는 학과입니다.");
        }

        department.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
}