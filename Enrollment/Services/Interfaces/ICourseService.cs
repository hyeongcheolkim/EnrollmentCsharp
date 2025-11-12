using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface ICourseService
{
    Task<Course> OpenAsync(CourseOpenRequest request, long professorId);
    Task<bool> CloseAsync(long courseId, long professorId);
    Task<bool> AddProhibitedDepartmentAsync(long courseId, long departmentId, long professorId);
}