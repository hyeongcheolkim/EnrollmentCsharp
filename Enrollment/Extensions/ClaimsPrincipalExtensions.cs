using System.Security.Claims;

namespace Enrollment.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var idString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idString == null || !long.TryParse(idString, out var id))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }
        return id;
    }
}