using CBPresents.Services.Contracts;
using CBPresents.Services.Implentations;

namespace CBPresents.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICurrentUser, CurrentUser>();
    }
}
