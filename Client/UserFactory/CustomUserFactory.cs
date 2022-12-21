using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;

namespace CBPresents.Client.UserFactory;

public class CustomUserFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
{
    public CustomUserFactory(IAccessTokenProviderAccessor accessor)
        : base(accessor)
    {

    }

    public async override ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);

        if (initialUser.Identity.IsAuthenticated)
        {
            var userIdentity = (ClaimsIdentity)initialUser.Identity;
            if (account?.Roles?.Length > 0)
            {
                foreach (var role in account?.Roles)
                {
                    userIdentity.AddClaim(new Claim("role", role));
                }
            }
            if (account?.Groups?.Length > 0)
            {
                foreach (var group in account?.Groups)
                {
                    userIdentity.AddClaim(new Claim("group", group));
                }
            }
        }

        return initialUser;
    }
}