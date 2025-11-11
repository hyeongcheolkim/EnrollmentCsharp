using AutoMapper;
using Enrollment.Data;
using Enrollment.Dtos.Requests;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CourseService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Course> OpenAsync(CourseOpenRequest request, long professorId)
    {
        var courseTime = request.CourseTime;

        await CheckDuplicateCourseTimeAndClassroom(courseTime, request.ClassroomId!.Value);
        
        var subject = await _context.Subjects.FindAsync(request.SubjectId) ?? throw new Exception("과목을 찾을 수 없습니다.");
        var department = await _context.Departments.FindAsync(request.DepartmentId) ?? throw new Exception("학과를 찾을 수 없습니다.");
        var professor = await _context.Professors.FindAsync(professorId) ?? throw new Exception("교수를 찾을 수 없습니다.");
        var classroom = await _context.Classrooms.FindAsync(request.ClassroomId) ?? throw new Exception("강의실을 찾을 수 없습니다.");
        
        var prohibitedDepartments = new List<Department>();
        if (request.ProhibitedDepartmentIds != null)
        {
            prohibitedDepartments = await _context.Departments
                .Where(d => request.ProhibitedDepartmentIds.Contains(d.Id))
                .ToListAsync();
        }
        
        var course = new Course
        {
            Subject = subject,
            Department = department,
            Professor = professor,
            Classroom = classroom,
            Capacity = request.Capacity,
            StudentYear = request.StudentYear,
            OpenYear = request.OpenYear,
            OpenSemester = request.OpenSemester,
            Division = request.Division,
            CourseTime = courseTime,
            ProhibitedDepartments = prohibitedDepartments,
            Activated = true
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }
    
    private async Task CheckDuplicateCourseTimeAndClassroom(CourseTime courseTime, long classroomId)
    {
        var day = courseTime.Day;
        var startHour = courseTime.StartHour;
        var endHour = courseTime.EndHour;

        var isDuplicated = await _context.Courses
            .Where(c => c.Activated && c.ClassroomId == classroomId && c.CourseTime.Day == day)
            .AnyAsync(c =>
                (c.CourseTime.StartHour <= startHour && startHour < c.CourseTime.EndHour) ||
                (c.CourseTime.StartHour < endHour && endHour <= c.CourseTime.EndHour) ||
                (startHour <= c.CourseTime.StartHour && c.CourseTime.StartHour < endHour) ||
                (startHour < c.CourseTime.EndHour && c.CourseTime.EndHour <= endHour)
            );

        if (isDuplicated)
        {
            throw new Exception("선택한 교실과 시간이 다른 코스와 겹칩니다.");
        }
    }
    
    public async Task<bool> CloseAsync(long courseId, long professorId)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
        {
            throw new Exception("강의를 찾을 수 없습니다."); 
        }

        if (course.ProfessorId != professorId)
        {
            throw new Exception("권한이 없습니다."); 
        }

        course.Activated = false;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AddProhibitedDepartmentAsync(long courseId, long departmentId, long professorId)
    {
        var course = await _context.Courses
            .Include(c => c.ProhibitedDepartments) 
            .FirstOrDefaultAsync(c => c.Id == courseId);
            
        if (course == null)
        {
            throw new Exception("강의를 찾을 수 없습니다.");
        }

        if (course.ProfessorId != professorId)
        {
            throw new Exception("권한이 없습니다.");
        }

        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
        {
            throw new Exception("학과를 찾을 수 없습니다.");
        }

        if (!course.ProhibitedDepartments.Contains(department))
        {
            course.ProhibitedDepartments.Add(department);
            await _context.SaveChangesAsync();
        }
        
        return true;
    }
}