using Enrollment.Data;
using Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Services;

public class BasketService : IBasketService
{
    private readonly ApplicationDbContext _context;

    public BasketService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Basket> PutAsync(long studentId, long courseId)
    {
        var student = await _context.Students.FindAsync(studentId) 
            ?? throw new Exception("학생을 찾을 수 없습니다.");
        
        var course = await _context.Courses
            .Include(c => c.Subject) 
            .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new Exception("강의를 찾을 수 없습니다.");
       
        var isDuplicated = await _context.Baskets
            .Include(b => b.Course.Subject) 
            .Include(b => b.Course.Professor) 
            .AnyAsync(b =>
                b.StudentId == studentId &&
                b.Course.Subject.Code == course.Subject.Code &&
                b.Course.Professor.Id == course.ProfessorId && 
                b.Course.Division == course.Division);

        if (isDuplicated)
        {
            throw new Exception("이미 장바구니에 담긴 강의입니다."); 
        }

        var basket = new Basket
        {
            Student = student,
            Course = course
        };

        _context.Baskets.Add(basket);
        await _context.SaveChangesAsync();
        
        return basket;
    }
    
    public async Task<bool> EraseAsync(long basketId, long studentId)
    {
        var basket = await _context.Baskets.FindAsync(basketId);
        
        if (basket == null)
        {
            throw new Exception("장바구니 항목을 찾을 수 없습니다."); 
        }
        
        if (basket.StudentId != studentId)
        {
            throw new Exception("삭제 권한이 없습니다."); 
        }

        _context.Baskets.Remove(basket);
        await _context.SaveChangesAsync();
        
        return true;
    }
}