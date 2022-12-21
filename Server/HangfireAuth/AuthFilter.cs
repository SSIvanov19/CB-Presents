using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Security.Claims;

namespace CBPresents.Server.HangfireAuth;

public class AuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return false;
        }

        List<Claim> roleClaims = httpContext.User.FindAll(ClaimTypes.Role).ToList();

        if (roleClaims.Any(c => c.Value == "Admin"))
        {
            return true;
        }

        return false;
    }
}
