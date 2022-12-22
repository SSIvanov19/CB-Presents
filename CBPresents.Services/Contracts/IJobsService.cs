namespace CBPresents.Services.Contracts;

public interface IJobsService
{
    Task ScheduleJob();

    Task RemoveJob();
}
