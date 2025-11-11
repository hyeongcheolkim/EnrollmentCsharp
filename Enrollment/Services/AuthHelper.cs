using System.Security.Claims;

namespace Enrollment.Services;

public class AuthHelper : IAuthHelper
{
    public ClaimsPrincipal CreateClaimsPrincipal(long userId, string name, string role)
    {
        var claims = new List<Claim>
        {
           
            new(ClaimTypes.NameIdentifier, userId.ToString()), 
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Role, role) 
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        return new ClaimsPrincipal(claimsIdentity);
    }
}