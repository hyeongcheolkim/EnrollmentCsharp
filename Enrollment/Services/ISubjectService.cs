using Enrollment.Dtos.Requests;
using Enrollment.Models;

namespace Enrollment.Services;

public interface ISubjectService
{
    Task<Subject> CreateAsync(SubjectMakeRequest request);
    Task<bool> InactiveAsync(long subjectId);
}