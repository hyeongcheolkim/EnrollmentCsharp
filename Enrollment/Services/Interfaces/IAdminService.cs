using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface IAdminService
{
    Task<Admin?> LoginAsync(string loginId, string pw);
    Task<Admin> RegisterAsync(AdminRegisterRequest request);
    Task<bool> InactiveAsync(long adminId);
}