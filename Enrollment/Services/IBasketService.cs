using Enrollment.Models;

namespace Enrollment.Services;

public interface IBasketService
{
    Task<Basket> PutAsync(long studentId, long courseId);
    Task<bool> EraseAsync(long basketId, long studentId); 
}