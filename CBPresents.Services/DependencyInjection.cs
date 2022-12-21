using CBPresents.Services.Contracts;
using CBPresents.Services.Implentations;
using Microsoft.Extensions.DependencyInjection;

namespace CBPresents.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped<ILotteryService, LotteryService>()
            .AddScoped<ITimeService, TimeService>()
            .AddScoped<INumberOfWinnersService, NumberOfWinnersService>()
            .AddScoped<IJobsService, JobsService>();
    }
}
