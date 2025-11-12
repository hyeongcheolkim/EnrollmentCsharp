using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface IStudentService
{
    Task<Student?> LoginAsync(string loginId, string pw);
    Task<Student> RegisterAsync(StudentRegisterRequest request);
    Task<bool> InactiveAsync(long studentId);
}