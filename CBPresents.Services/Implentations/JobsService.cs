using CBPresents.Data.Data;
using CBPresents.Services.Contracts;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CBPresents.Services.Implentations;

public class JobsService : IJobsService
{
    private readonly ApplicationDBContext context;
    private readonly IBackgroundJobClient jobClient;
    private readonly ILotteryService lotteryService;
    private readonly ITimeService timeService;
    private readonly INumberOfWinnersService numberOfWinnersService;

    public JobsService(
        ApplicationDBContext context,
        IBackgroundJobClient jobClient,
        ILotteryService lotteryService,
        ITimeService timeService,
        INumberOfWinnersService numberOfWinnersService)
    {
        this.context = context;
        this.jobClient = jobClient;
        this.lotteryService = lotteryService;
        this.timeService = timeService;
        this.numberOfWinnersService = numberOfWinnersService;
    }

    public async Task ScheduleJob()
    {
        var job = await context.Jobs.FirstOrDefaultAsync();

        if (job != null)
        {
            jobClient.Delete(job.JobId);
            this.context.Jobs.RemoveRange(this.context.Jobs);
        }

        var time = await this.timeService.GetTime() ?? default;

        var timespan = time - DateTime.Now;

        var newJobId = jobClient.Schedule(() => PickWinners(), timespan);

        await this.context.Jobs.AddAsync(new Data.Models.Job()
        {
            Id = Guid.NewGuid().ToString(),
            JobId = newJobId
        });

        await this.context.SaveChangesAsync();
    }

    public async Task PickWinners()
    {
        var numberOfWinners = await this.numberOfWinnersService.GetNumberOfWinners();
        await lotteryService.PickWinners(numberOfWinners ?? 50);
    }

    public async Task RemoveJob()
    {
        var job = await context.Jobs.FirstOrDefaultAsync();

        if (job != null)
        {
            jobClient.Delete(job.JobId);
            this.context.Jobs.RemoveRange(this.context.Jobs);
        }
    }
}
