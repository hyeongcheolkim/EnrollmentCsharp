using Enrollment.Data;
using Enrollment.Extensions;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _context;

    public EnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Dictionary<long, bool>> EnrollBasketsAsync(long studentId, List<long> basketIds)
    {
        var results = new Dictionary<long, bool>();
        
        var basketsToEnroll = await _context.Baskets
            .Where(b => b.StudentId == studentId && basketIds.Contains(b.Id))
            .ToListAsync();

        foreach (var basket in basketsToEnroll)
        {
            try
            {
                await EnrollAsync(studentId, basket.CourseId);
                results[basket.Id] = true;
            }
            catch (Exception)
            {
                results[basket.Id] = false;
            }
        }
        var successfulBaskets = basketsToEnroll.Where(b => results.ContainsKey(b.Id) && results[b.Id]).ToList();
        if (successfulBaskets.Any())
        {
            _context.Baskets.RemoveRange(successfulBaskets);
            await _context.SaveChangesAsync();
        }

        return results;
    }
    public async Task<Enroll> EnrollAsync(long studentId, long courseId)
    {
        var student = await _context.Students
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == studentId) 
            ?? throw new Exception("학생을 찾을 수 없습니다.");

        var course = await _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.ProhibitedDepartments)
            .FirstOrDefaultAsync(c => c.Id == courseId) 
            ?? throw new Exception("강의를 찾을 수 없습니다.");
        
        var studentEnrollments = await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course.Subject)
            .Include(e => e.Course.CourseTime)
            .ToListAsync();

        var currentEnrollCount = await _context.Enrollments.CountAsync(e => e.CourseId == courseId);
        if (currentEnrollCount >= course.Capacity)
        {
            throw new Exception("코스 인원이 꽉 찼습니다. 수강신청할 수 없습니다");
        }

        if (course.ProhibitedDepartments.Any(d => d.Code == student.Department.Code))
        {
            throw new Exception("수강 금지과입니다");
        }

        if (studentEnrollments.Any(e => e.OnSemester && e.Course.Subject.Code == course.Subject.Code))
        {
            throw new Exception("이미 신청한 과목입니다");
        }

        if (studentEnrollments.Any(e => !e.OnSemester && 
                                        e.Course.Subject.Code == course.Subject.Code && 
                                        e.ScoreType.HasValue &&
                                        e.ScoreType.Value.GetDigit() >= ScoreType.B_ZERO.GetDigit()))
        {
            throw new Exception("이미 수강한 과목입니다, 재수강은 B0 미만만 가능합니다");
        }
        
        var newTime = course.CourseTime;
        var existingTimes = studentEnrollments
            .Where(e => e.OnSemester && e.Course.CourseTime.Day == newTime.Day)
            .Select(e => e.Course.CourseTime);

        if (existingTimes.Any(e => (e.StartHour <= newTime.StartHour && newTime.StartHour < e.EndHour) ||
                                   (e.StartHour < newTime.EndHour && newTime.EndHour <= e.EndHour) ||
                                   (newTime.StartHour <= e.StartHour && e.StartHour < newTime.EndHour) ||
                                   (newTime.StartHour < e.EndHour && e.EndHour <= newTime.EndHour)))
        {
            throw new Exception("신청한 과목의 수업시간이 기존 시간표와 중복됩니다");
        }
        
        var enroll = new Enroll
        {
            Student = student,
            Course = course,
            OnSemester = true
        };

        _context.Enrollments.Add(enroll);
        await _context.SaveChangesAsync();
        return enroll;
    }


    public async Task<bool> DropAsync(long studentId, long enrollId)
    {
        var enroll = await _context.Enrollments.FindAsync(enrollId);
        if (enroll == null)
        {
            throw new Exception("수강 내역을 찾을 수 없습니다.");
        }

        if (enroll.StudentId != studentId)
        {
            throw new Exception("권한이 없습니다.");
        }

        _context.Enrollments.Remove(enroll);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> GradeAsync(long professorId, long enrollId, ScoreType scoreType)
    {
        var enroll = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student) 
            .Include(e => e.Course.Subject) 
            .FirstOrDefaultAsync(e => e.Id == enrollId);
            
        if (enroll == null)
        {
            throw new Exception("수강 내역을 찾을 수 없습니다.");
        }

        if (enroll.Course.ProfessorId != professorId)
        {
            throw new Exception("권한이 없습니다.");
        }
        
        var pastEnrollment = await _context.Enrollments
            .Where(e => e.StudentId == enroll.StudentId &&
                        !e.OnSemester && 
                        e.Course.Subject.Code == enroll.Course.Subject.Code)
            .FirstOrDefaultAsync();

        if (pastEnrollment != null)
        {
            _context.Enrollments.Remove(pastEnrollment);
        }
        
        enroll.OnSemester = false;
        enroll.ScoreType = scoreType;
        
        await _context.SaveChangesAsync();
        return true;
    }
}