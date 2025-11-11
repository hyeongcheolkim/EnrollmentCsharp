using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface IProfessorService
{
    Task<Professor?> LoginAsync(string loginId, string pw);
    Task<Professor> RegisterAsync(ProfessorRegisterRequest request);
    Task<bool> InactiveAsync(long professorId);
}