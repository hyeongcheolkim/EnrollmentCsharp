using System.Security.Claims;

namespace Enrollment.Services;

public interface IAuthHelper
{
    ClaimsPrincipal CreateClaimsPrincipal(long userId, string name, string role);
}