using Enrollment.Models;

namespace Enrollment.Services;

public interface IClassroomService
{
    Task<Classroom> CreateAsync(string name, int code);
    Task<bool> InactiveAsync(long classroomId);
}