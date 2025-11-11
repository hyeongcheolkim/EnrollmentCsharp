using Enrollment.Models;

namespace Enrollment.Services;

public interface IEnrollmentService
{
    Task<Enroll> EnrollAsync(long studentId, long courseId);
    Task<bool> DropAsync(long studentId, long enrollId);
    Task<bool> GradeAsync(long professorId, long enrollId, ScoreType scoreType);
    Task<Dictionary<long, bool>> EnrollBasketsAsync(long studentId, List<long> basketIds);
}