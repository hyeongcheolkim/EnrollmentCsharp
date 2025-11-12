using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface IDepartmentService
{
    Task<Department> CreateAsync(CreateDepartmentRequest request);
    Task<bool> InactiveAsync(long departmentId);
}