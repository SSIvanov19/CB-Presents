using CBPresents.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace CBPresents.Services.Implentations;

internal class CurrentUser : ICurrentUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        this.UserEmail = httpContextAccessor?
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(c => c.Type == ClaimConstants.PreferredUserName)?
            .Value!;

        this.Role = httpContextAccessor?
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(c => c.Type == ClaimConstants.Roles)?
            .Value!;
    }
    public string UserEmail { get; }

    public string Role { get; }
}
